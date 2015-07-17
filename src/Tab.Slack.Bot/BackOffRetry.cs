using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tab.Slack.Bot
{
    [Export(typeof(IBackOffStrategy))]
    public class BackOffRetry : IBackOffStrategy
    {
        private Stopwatch stopwatch;
        private int backOffStage = 0;

        public List<int> BackOffStages { get; set; } = new List<int>
        {
            0, 1000, 2000, 5000, 10000, 30000, 60000
        };

        public BackOffRetry()
        {
            this.stopwatch = new Stopwatch();
            this.stopwatch.Start();
        }

        public void BlockingRetry()
        {
            var retryWait = GetRetryTime();

            if (retryWait == 0)
                return;

            Console.WriteLine($"Sleep: {retryWait}");
            Thread.Sleep(retryWait);
        }

        private int GetRetryTime()
        {
            this.stopwatch.Stop();
            var timeElapsedSinceLastRetry = this.stopwatch.ElapsedMilliseconds;

            var isAtMaxStage = (this.backOffStage + 1) == this.BackOffStages.Count;
            var nextStage = Math.Min(this.backOffStage + 1, this.BackOffStages.Count - 1);
            var stageTime = this.BackOffStages[nextStage];

            if (isAtMaxStage)
                stageTime *= 2;

            //Console.WriteLine($"{timeElapsedSinceLastRetry} > {stageTime} {isAtMaxStage} {nextStage}");

            if (timeElapsedSinceLastRetry > stageTime)
            {
                this.backOffStage = 0;
                stageTime = 0;
            }
            else
            {
                this.backOffStage = nextStage;

                if (!isAtMaxStage)
                    nextStage--;

                stageTime = this.BackOffStages[nextStage];
            }

            this.stopwatch.Restart();

            return stageTime;
        }
    }
}
