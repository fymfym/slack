using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Tab.Slack.Common.Model;

namespace Tab.Slack.Bot
{
    public interface IBotServices : IDisposable
    {
        void SendMessage(string channel, string text);
        void SendRawMessage(OutputMessage message);
        IEnumerable<OutputMessage> GetBlockingOutputEnumerable(CancellationToken cancellationToken);
    }
}
