using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model.Events;
using Tab.Slack.Common.Model.Events.Messages;
using Tab.Slack.Bot.Integration;
using System.Threading;
using Tab.Slack.Common.Model;

namespace Tab.Slack.Bot.CoreHandlers
{
    [Export(typeof(IMessageHandler))]
    public class PingHandler : MessageHandlerBase, IMessageHandler
    {
        private Timer pingTimer;
        private int pingFrequencyMs = 8000;

        public int PingFrequencyMs
        {
            get
            {
                return this.pingFrequencyMs;
            }

            set
            {
                this.pingFrequencyMs = value;
                ResetTimer();
            }
        }

        public PingHandler()
        {
            this.pingTimer = new Timer(SendPing, null, this.pingFrequencyMs, this.pingFrequencyMs);
        }

        public bool CanHandle(EventMessageBase message)
        {
            ResetTimer();
            return message.Type == EventType.Pong;
        }

        public Task<ProcessingChainResult> HandleMessageAsync(EventMessageBase message)
        {
            // do something? check latency maybe?

            return Task.FromResult(ProcessingChainResult.Continue);
        }

        private void ResetTimer()
        {
            this.pingTimer.Change(this.pingFrequencyMs, this.pingFrequencyMs);
        }

        private void SendPing(object state)
        {
            try
            {
                if (this.BotState.Connected)
                    this.BotServices.SendRawMessage(new OutputMessage { Type = "ping" });
            }
            catch
            {
                // todo: logging
            }
        }
    }
}
