using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Tab.Slack.Common.Json;
using Tab.Slack.Common.Model;
using Tab.Slack.Common.Model.Events;
using Tab.Slack.Bot.Integration;
using WebSocket4Net;
using Tab.Slack.Rest;

namespace Tab.Slack.Bot
{
    public class BotClient : IBotClient
    {
        private bool started;
        private CancellationTokenSource cancellationTokenSource;
        private readonly IEnumerable<IMessageHandler> messageHandlers;
        private readonly IBotState slackState;
        private readonly IBotServices slackService;
        private readonly Config clientConfig;
        private readonly IResponseParser responseParser;
        private readonly ISlackClient slackRestClient;

        // TODO: we don't have enough constructor args - WE NEED MOAR!!!
        public BotClient(Config clientConfig, IEnumerable<IMessageHandler> messageHandlers,
            IBotState slackState, IBotServices slackService, IResponseParser responseParser,
            ISlackClient slackRestClient)
        {
            if (clientConfig == null)
                throw new ArgumentNullException(nameof(clientConfig));

            if (string.IsNullOrWhiteSpace(clientConfig.ApiKey))
                throw new FormatException($"{nameof(clientConfig.ApiKey)}: is missing");
            
            if (messageHandlers == null)
                throw new ArgumentNullException(nameof(messageHandlers));

            if (slackState == null)
                throw new ArgumentNullException(nameof(slackState));

            if (slackService == null)
                throw new ArgumentNullException(nameof(slackService));

            if (responseParser == null)
                throw new ArgumentNullException(nameof(responseParser));

            if (slackRestClient == null)
                throw new ArgumentNullException(nameof(slackRestClient));

            this.clientConfig = clientConfig;
            this.messageHandlers = messageHandlers.OrderByDescending(m => m.Priority).ToArray();
            this.slackState = slackState;
            this.slackService = slackService;
            this.responseParser = responseParser;
            this.slackRestClient = slackRestClient;
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

            var rtmStartResponse = this.slackRestClient.RtmStart();
            
            // TODO: handle
            if (rtmStartResponse == null || !rtmStartResponse.Ok)
                return;
            
            OfferMessageToHandlersAsync(rtmStartResponse);

            using (var ws = new WebSocket(rtmStartResponse.Url))
            {
                ws.Opened += OnOpened;
                ws.Closed += OnClosed;
                ws.MessageReceived += OnMessageReceived;
                ws.Open();

                ProcessSendQueue(ws);
            }
        }

        private void OnClosed(object sender, EventArgs e)
        {
            this.slackState.Connected = false;

            if (this.cancellationTokenSource != null && cancellationTokenSource.Token.CanBeCanceled)
                this.cancellationTokenSource.Cancel();
        }

        private void OnOpened(object sender, EventArgs e)
        {
            this.slackState.Connected = true;
        }

        private void ProcessSendQueue(WebSocket webSocket)
        {
            var blockingSendQueue = this.slackService.GetBlockingOutputEnumerable(this.cancellationTokenSource.Token);

            foreach (var message in blockingSendQueue)
            {
                Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Output: {message.Text}");
                webSocket.Send(this.responseParser.SerializeMessage(message));
            }
        }

        private void OnMessageReceived(object sender, MessageReceivedEventArgs args)
        {
            var eventMessage = this.responseParser.DeserializeEvent(args.Message);

            // todo: handle
            if (eventMessage == null)
                return;

            OfferMessageToHandlersAsync(eventMessage);
        }

        private async void OfferMessageToHandlersAsync(EventMessageBase message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Message: {message.GetType().Name}");
            var interestedHandlers = this.messageHandlers.Where(h => h.CanHandle(message));

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
        }
    }
}
