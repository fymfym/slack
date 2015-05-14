using System;
using System.Threading;

namespace Tab.Slack.Bot
{
    public interface IBotClient : IDisposable
    {
        void Start();
        void Start(CancellationTokenSource cancellationTokenSource);
    }
}