using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Tab.Slack.Common.Json;
using Tab.Slack.Common.Model.Events;
using Tab.Slack.Bot.Integration;
using WebSocket4Net;
using Tab.Slack.WebApi;
using SuperSocket.ClientEngine;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using Tab.Slack.Common.Model;
using log4net;
using System.Reflection;
using System.Collections;

namespace Tab.Slack.Bot
{
    public class SlackBot : ISlackBot
    {
        private bool started;
        private string apiKey;
        private CancellationTokenSource cancellationTokenSource;
        private WebSocket slackSocket;
        
        [ImportMany]
        public IEnumerable<IMessageHandler> MessageHandlers { get; set; }
        [Import]
        public IBotState SlackState { get; set; }
        [Import]
        public IBotServices SlackService { get; set; }
        [Import]
        public IResponseParser ResponseParser { get; set; }
        [Import]
        public ISlackApi SlackApi { get; set; }
        [Import]
        public IBackOffStrategy BackOffStrategy { get; set; }
        [Import]
        public ILog Logger { get; set; }

        public bool AutoReconnect { get; set; } = true;
        public bool StrictProtocolWarnings { get; set; }

        private SlackBot(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public static ISlackBotBuilder Create(string apiKey)
        {
            return new SlackBotBuilder(new SlackBot(apiKey), apiKey);
        }

        public Task Start()
        {
            if (this.started)
                throw new InvalidOperationException("Client can only be started once");

            this.cancellationTokenSource = new CancellationTokenSource();

            return StartInternal();
        }

        private Task StartInternal()
        {
            this.started = true;

            if (this.Logger.IsInfoEnabled)
            {
                this.Logger.Info("Starting up bot");

                foreach (var handler in this.MessageHandlers)
                {
                    this.Logger.Info($"Loaded handler: {handler.GetType().Name}");
                }
            }

            var rtmStartResponse = this.SlackApi.Rtm.Start();

            if (rtmStartResponse == null || !rtmStartResponse.Ok)
            {
                this.Logger.Error($"Failed to establish RTM session. {rtmStartResponse?.Error}");
                TryReconnect();
            }
            
            OfferMessageToHandlersAsync(rtmStartResponse, nameof(rtmStartResponse));

            this.slackSocket = new WebSocket(rtmStartResponse.Url);
            this.slackSocket.Error += OnError;
            this.slackSocket.Opened += OnOpened;
            this.slackSocket.Closed += OnClosed;
            this.slackSocket.MessageReceived += OnMessageReceived;
            this.slackSocket.Open();

            return ProcessSendQueue(this.slackSocket);
        }

        public void Stop()
        {
            try
            {
                if (this.cancellationTokenSource != null && cancellationTokenSource.Token.CanBeCanceled)
                {
                    this.cancellationTokenSource.Cancel();
                    this.cancellationTokenSource.Dispose();
                }
                
            }
            catch { }
            finally
            {
                this.cancellationTokenSource = null;
            }

            Disconnect();
            this.started = false;
        }

        private void Disconnect()
        {
            if (this.slackSocket != null)
            {
                try
                {
                    this.slackSocket.Dispose();
                }
                catch { }
                finally
                {
                    this.slackSocket = null;
                }
            }

            this.SlackState.Connected = false;
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            this.Logger.Error("Websocket error", e.Exception);
            TryReconnect();
        }

        private void OnClosed(object sender, EventArgs e)
        {
            this.Logger.Warn("Websocket closed connection");
            TryReconnect();
        }

        private void TryReconnect()
        {
            Stop();

            if (this.AutoReconnect)
            {
                this.Logger.Warn("Attempting reconnect...");
                this.BackOffStrategy.BlockingRetry();
                
                Start();
            }
        }

        private void OnOpened(object sender, EventArgs e)
        {
            this.SlackState.Connected = true;
        }

        private Task ProcessSendQueue(WebSocket webSocket)
        {
            return Task.Factory.StartNew(() =>
            {
                var blockingSendQueue = this.SlackService.GetBlockingOutputEnumerable(this.cancellationTokenSource.Token);

                try
                {
                    foreach (var message in blockingSendQueue)
                    {
                        CheckAndSendMessage(message, webSocket);
                    }
                }
                catch (OperationCanceledException)
                {
                    // safe to ignore and exit
                }
            }, this.cancellationTokenSource.Token);
        }

        private void CheckAndSendMessage(OutputMessage message, WebSocket webSocket)
        {
            var serializedMessage = this.ResponseParser.SerializeMessage(message);
            var maxByteCount = Encoding.UTF8.GetByteCount(serializedMessage) + 2;

            if (maxByteCount > 16000)
            {
                this.Logger.Error($"Message exceeded 16kb size limit and was ignored: {serializedMessage.Substring(0, 16000)}");
                return;
            }

            webSocket.Send(serializedMessage);

            this.Logger.Debug($"Output: {serializedMessage}");
        }

        private void OnMessageReceived(object sender, MessageReceivedEventArgs args)
        {
            this.Logger.Debug($"Input: {args.Message}");
            var eventMessage = this.ResponseParser.DeserializeEvent(args.Message);

            if (eventMessage == null)
            {
                this.Logger.Warn($"Failed to parse received message: {args.Message}");
                return;
            }

            this.Logger.Debug($"Parsed input as: {eventMessage.Type}");

            if (this.StrictProtocolWarnings)
                CheckModelForProtocolErrors(eventMessage, eventMessage.Type.ToString());

            OfferMessageToHandlersAsync(eventMessage, args.Message);
        }

        // todo: this doesn't belong here
        private void CheckModelForProtocolErrors(FlexibleJsonModel model, string path)
        {
            if (model == null)
                return;

            if (model.UnmatchedAdditionalJsonData != null && model.UnmatchedAdditionalJsonData.HasValues)
                this.Logger.Warn($"Unmatched Model Data <{path}>: {model.UnmatchedAdditionalJsonData.ToString()}");

            foreach (var prop in model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (prop.PropertyType.IsGenericType &&
                    typeof(FlexibleJsonModel).IsAssignableFrom(prop.PropertyType.GetGenericArguments()[0]) &&
                    typeof(IEnumerable).IsAssignableFrom(prop.PropertyType)
                    ) 
                {
                    var values = prop.GetValue(model) as IEnumerable;

                    if (values != null)
                    {
                        foreach (var item in values)
                        {
                            CheckModelForProtocolErrors(item as FlexibleJsonModel, $"{path}.{prop.Name}[]");
                        }
                    }
                }
                else if (typeof(FlexibleJsonModel).IsAssignableFrom(prop.PropertyType))
                {
                    var value = prop.GetValue(model);
                    CheckModelForProtocolErrors(value as FlexibleJsonModel, $"{path}.{prop.Name}");
                }
            }
        }

        private async void OfferMessageToHandlersAsync(EventMessageBase message, string originalMessage)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            if (this.MessageHandlers == null)
                return;

            var interestedHandlers = this.MessageHandlers.Where(h => SafelyHandles(h, message, originalMessage));

            foreach (var handler in interestedHandlers)
            {
                try
                {
                    this.Logger.Debug($"{handler.GetType().Name} handling: {message.Type}");
                    var result = await handler.HandleMessageAsync(message);

                    if (result == ProcessingChainResult.Stop)
                        break;
                }
                catch (Exception ex)
                {
                    this.Logger.Error($"{handler.GetType().Name} threw exception when handling message: {originalMessage}", ex);
                }
            }
        }

        public void Dispose()
        {
            try
            {
                Stop();
            }
            finally
            {
                if (this.SlackService != null)
                {
                    try
                    {
                        this.SlackService.Dispose();
                    }
                    finally
                    {
                        this.SlackService = null;
                    }
                }
            }
        }

        private bool SafelyHandles(IMessageHandler handler, EventMessageBase message, string originalMessage)
        {
            var handles = false;

            try
            {
                handles = handler.CanHandle(message);
            }
            catch (Exception ex)
            {
                this.Logger.Error($"{handler.GetType().Name} threw exception in CanHandle() with message: {originalMessage}", ex);
            }

            return handles;
        }
    }
}
