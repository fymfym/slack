using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tab.Slack.Bot.Tests
{
    public class BackOffRetryTests
    {
        [Fact]
        public void FirstRetryHitsFirstStage()
        {
            var backoff = new BackOffRetry();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            backoff.BlockingRetry();

            var timeTaken = stopwatch.ElapsedMilliseconds;
            Assert.InRange(timeTaken, 0, 100);
        }

        [Fact]
        public void SecondRetryHitsSecondStage()
        {
            var backoff = new BackOffRetry();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            backoff.BlockingRetry();
            backoff.BlockingRetry();

            var timeTaken = stopwatch.ElapsedMilliseconds;
            Assert.InRange(timeTaken, 800, 1200);
        }

        [Fact]
        public void ResetsStageAfterDelay()
        {
            var backoff = new BackOffRetry();
            backoff.BackOffStages = new List<int> { 0, 500, 1000, 2000 };

            backoff.BlockingRetry();
            backoff.BlockingRetry();
            Thread.Sleep(3000);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            backoff.BlockingRetry();

            var timeTaken = stopwatch.ElapsedMilliseconds;
            Assert.InRange(timeTaken, 0, 100);
        }

        [Fact]
        public void StaysAtMaxStageAfterRepeatedRetries()
        {
            var backoff = new BackOffRetry();
            backoff.BackOffStages = new List<int> { 0, 0, 0, 500 };

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            backoff.BlockingRetry();
            backoff.BlockingRetry();
            backoff.BlockingRetry();
            backoff.BlockingRetry();
            backoff.BlockingRetry();

            var timeTaken = stopwatch.ElapsedMilliseconds;
            Assert.InRange(timeTaken, 800, 1200);
        }
    }
}
