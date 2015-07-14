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

        public bool AutoReconnect { get; set; } = true;

        private SlackBot(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public static ISlackBotBuilder Build(string apiKey)
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
            var rtmStartResponse = this.SlackApi.RtmStart();
            
            if (rtmStartResponse == null || !rtmStartResponse.Ok)
                throw new Exception($"Failed to establish RTM session. {rtmStartResponse?.Error}");
            
            OfferMessageToHandlersAsync(rtmStartResponse);

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
            Console.WriteLine(e.Exception.ToString());
            Stop();
            TryReconnect();
        }

        private void OnClosed(object sender, EventArgs e)
        {
            Stop();
            TryReconnect();
        }

        private void TryReconnect()
        {
            if (this.AutoReconnect)
            {
                // todo: handle rate limiting etc
                Console.WriteLine("Attempting reconnect...");
                Thread.Sleep(30000);
                
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
                        Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Output: {message.Text}");
                        webSocket.Send(this.ResponseParser.SerializeMessage(message));
                    }
                }
                catch (OperationCanceledException)
                {
                    // safe to ignore and exit
                }
            }, this.cancellationTokenSource.Token);
        }

        private void OnMessageReceived(object sender, MessageReceivedEventArgs args)
        {
            var eventMessage = this.ResponseParser.DeserializeEvent(args.Message);

            // todo: handle
            if (eventMessage == null)
                return;

            OfferMessageToHandlersAsync(eventMessage);
        }

        private async void OfferMessageToHandlersAsync(EventMessageBase message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            if (this.MessageHandlers == null)
                return;

            Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Message: {message.GetType().Name}");
            var interestedHandlers = this.MessageHandlers.Where(h => h.CanHandle(message));

            foreach (var handler in interestedHandlers)
            {
                try
                {
                    var result = await handler.HandleMessageAsync(message);

                    if (result == ProcessingChainResult.Stop)
                        break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{handler.GetType().Name} threw exception when handling message: {ex}");
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
    }
}
