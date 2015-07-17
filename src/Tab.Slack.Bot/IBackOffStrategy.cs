using System.Collections.Generic;

namespace Tab.Slack.Bot
{
    public interface IBackOffStrategy
    {
        List<int> BackOffStages { get; set; }

        void BlockingRetry();
    }
}