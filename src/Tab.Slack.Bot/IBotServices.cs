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
        long SendMessage(string channel, string text);
        long SendRawMessage(OutputMessage message);
        IEnumerable<OutputMessage> GetBlockingOutputEnumerable(CancellationToken cancellationToken);
    }
}
