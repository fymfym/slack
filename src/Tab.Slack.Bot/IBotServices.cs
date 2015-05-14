using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Tab.Slack.Common.Model;

namespace Tab.Slack.Bot
{
    public interface IBotServices
    {
        void SendMessage(string channel, string text);
        IEnumerable<OutputMessage> GetBlockingOutputEnumerable(CancellationToken cancellationToken);
    }
}
