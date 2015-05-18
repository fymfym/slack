using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tab.Slack.Common.Model;

namespace Tab.Slack.Bot
{
    [Export(typeof(IBotServices))]
    public class SlackSendQueueService : IBotServices
    {
        private long outgoingMessageId = 0;
        private BlockingCollection<OutputMessage> outputMessageQueue;

        public SlackSendQueueService()
            : this(null)
        { }

        public SlackSendQueueService(IProducerConsumerCollection<OutputMessage> collection)
        {
            if (collection == null)
                this.outputMessageQueue = new BlockingCollection<OutputMessage>();
            else
                this.outputMessageQueue = new BlockingCollection<OutputMessage>(collection);
        }

        public void SendMessage(string channel, string text)
        {
            if (string.IsNullOrWhiteSpace(channel))
                throw new ArgumentNullException(nameof(channel));

            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentNullException(nameof(text));

            var messageId = Interlocked.Increment(ref this.outgoingMessageId);
            this.outputMessageQueue.Add(new OutputMessage(messageId, channel, text));
        }

        public IEnumerable<OutputMessage> GetBlockingOutputEnumerable(CancellationToken cancellationToken)
        {
            return this.outputMessageQueue.GetConsumingEnumerable(cancellationToken);
        }

        public void Dispose()
        {
            if (this.outputMessageQueue != null)
            {
                try
                {
                    this.outputMessageQueue.Dispose();
                }
                finally
                {
                    this.outputMessageQueue = null;
                }
            }
        }
    }
}
