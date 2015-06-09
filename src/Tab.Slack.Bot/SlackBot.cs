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

namespace Tab.Slack.Bot
{
    public class SlackBot : ISlackBot
    {
        private bool started;
        private string apiKey;
        private CancellationTokenSource cancellationTokenSource;

        public IEnumerable<IMessageHandler> MessageHandlers { get; set; }
        public IBotState SlackState { get; set; }
        public IBotServices SlackService { get; set; }
        public IResponseParser ResponseParser { get; set; }
        public ISlackApi SlackApi { get; set; }

        private SlackBot(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public static ISlackBot CreateWithoutDependencies(string apiKey)
        {
            return new SlackBot(apiKey);
        }

        public static ISlackBot Create(string apiKey, string pluginDirectoryPath = null, bool includeCoreHandlers = true)
        {
            ISlackBot slackBot = new SlackBot(apiKey);
            slackBot = Bootstrap.BuildSlackBot(slackBot, apiKey, pluginDirectoryPath, includeCoreHandlers);

            return slackBot;
        }

        public void Start()
        {
            using (var cancellationSource = new CancellationTokenSource())
            {
                Start(cancellationSource);
            }
        }

        public void Start(CancellationTokenSource cancellationTokenSource)
        {
            if (this.started)
                throw new InvalidOperationException("Client can only be started once");

            this.started = true;

            if (cancellationTokenSource == null)
                throw new ArgumentNullException(nameof(cancellationTokenSource));
            
            this.cancellationTokenSource = cancellationTokenSource;

            var rtmStartResponse = this.SlackApi.RtmStart();
            
            // TODO: handle
            if (rtmStartResponse == null || !rtmStartResponse.Ok)
                return;
            
            OfferMessageToHandlersAsync(rtmStartResponse);

            using (var ws = new WebSocket(rtmStartResponse.Url))
            {
                ws.Error += OnError;
                ws.Opened += OnOpened;
                ws.Closed += OnClosed;
                ws.MessageReceived += OnMessageReceived;
                ws.Open();

                ProcessSendQueue(ws);
            }
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine(e.Exception.ToString());
        }

        private void OnClosed(object sender, EventArgs e)
        {
            this.SlackState.Connected = false;

            if (this.cancellationTokenSource != null && cancellationTokenSource.Token.CanBeCanceled)
                this.cancellationTokenSource.Cancel();
        }

        private void OnOpened(object sender, EventArgs e)
        {
            this.SlackState.Connected = true;
        }

        private void ProcessSendQueue(WebSocket webSocket)
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
                var result = await handler.HandleMessageAsync(message);
                
                if (result == ProcessingChainResult.Stop)
                    break;
            }
        }

        public void Dispose()
        {
            if (this.cancellationTokenSource != null && cancellationTokenSource.Token.CanBeCanceled)
                cancellationTokenSource.Cancel();

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
