﻿using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tab.Slack.Common.Model;
using Xunit;

namespace Tab.Slack.Bot.Tests
{
    public class SlackSendQueueServiceTests
    {
        [Fact]
        public void CanSafelyCancelBlockingEnumerable()
        {
            var queueService = new SlackSendQueueService();
            var cancellationTokenSource = new CancellationTokenSource();

            var enumerable = queueService.GetBlockingOutputEnumerable(cancellationTokenSource.Token);

            var task = Task.Run(() => enumerable.GetEnumerator().MoveNext());

            Thread.Sleep(100);
            Assert.False(task.IsCompleted);

            cancellationTokenSource.Cancel();

            Thread.Sleep(50);
            Assert.True(task.IsCompleted);
        }

        [Fact]
        public void SendMessageEnqueuesItem()
        {
            OutputMessage sentMessage = null;

            var mockSource = new Mock<IProducerConsumerCollection<OutputMessage>>();
            mockSource.Setup(x => x.TryAdd(It.IsAny<OutputMessage>()))
                      .Callback<OutputMessage>(o => sentMessage = o)
                      .Returns(true)
                      .Verifiable();

            var queueService = new SlackSendQueueService(mockSource.Object);

            var messageId = queueService.SendMessage("channel", "message");

            mockSource.Verify();
            Assert.Equal("channel", sentMessage.Channel);
            Assert.Equal("message", sentMessage.Text);
            Assert.Equal(sentMessage.Id, messageId);
        }

        [Fact]
        public void SendRawMessageEnqueuesItem()
        {
            OutputMessage sentMessage = null;

            var mockSource = new Mock<IProducerConsumerCollection<OutputMessage>>();
            mockSource.Setup(x => x.TryAdd(It.IsAny<OutputMessage>()))
                      .Callback<OutputMessage>(o => sentMessage = o)
                      .Returns(true)
                      .Verifiable();

            var queueService = new SlackSendQueueService(mockSource.Object);

            var messageId = queueService.SendRawMessage(new OutputMessage { Channel = "channel", Text = "message" });

            mockSource.Verify();
            Assert.Equal("channel", sentMessage.Channel);
            Assert.Equal("message", sentMessage.Text);
            Assert.Equal(sentMessage.Id, messageId);
        }

        [Fact]
        public void SendRawMessageInrementsMessageId()
        {
            var sentMessages = new List<OutputMessage>();

            var mockSource = new Mock<IProducerConsumerCollection<OutputMessage>>();
            mockSource.Setup(x => x.TryAdd(It.IsAny<OutputMessage>()))
                      .Callback<OutputMessage>(o => sentMessages.Add(o))
                      .Returns(true);

            var queueService = new SlackSendQueueService(mockSource.Object);

            queueService.SendRawMessage(new OutputMessage());
            queueService.SendRawMessage(new OutputMessage());
            queueService.SendRawMessage(new OutputMessage());

            Assert.Equal(3, sentMessages.Count);
            Assert.Equal(1, sentMessages[0].Id);
            Assert.Equal(2, sentMessages[1].Id);
            Assert.Equal(3, sentMessages[2].Id);
        }
    }
}
