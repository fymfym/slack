using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Tab.Slack.Bot.Integration;
using Tab.Slack.Common.Model;
using Tab.Slack.Common.Model.Events;
using Tab.Slack.Common.Model.Events.Messages;
using Xunit;

namespace Tab.Slack.Bot.CoreHandlers.Tests
{
    public class PingHandlerTests
    {
        [Fact]
        public void CanHandlePongMessages()
        {
            var selfData = new SelfBotData { Id = "BOTID" };

            var message = new Pong
            {
                Type = EventType.Pong
            };

            var handler = new PingHandler();

            var result = handler.CanHandle(message);

            Assert.True(result);
        }

        [Fact]
        public void CanNotHandleInvalidPingMessages()
        {
            var selfData = new SelfBotData { Id = "BOTID" };

            var message = new PlainMessage
            {
                Type = EventType.Message,
                Subtype = MessageSubType.PlainMessage,
                Text = "<@BOTID>: pingdom"
            };

            var handler = new PingHandler();

            var result = handler.CanHandle(message);

            Assert.False(result);
        }

        [Fact]
        public void SendsPingAfterGivenTime()
        {
            var selfData = new SelfBotData { Id = "BOTID" };

            var mockState = new Mock<IBotState>();
            mockState.Setup(m => m.Connected).Returns(true).Verifiable();

            var mockService = new Mock<IBotServices>();
            mockService.Setup(m => m.SendRawMessage(It.Is<OutputMessage>(om => om.Type == "ping")))
                       .Verifiable();

            var handler = new PingHandler
            {
                BotState = mockState.Object,
                BotServices = mockService.Object,
                PingFrequencyMs = 200
            };

            Thread.Sleep(500);

            mockService.Verify(m => m.SendRawMessage(It.Is<OutputMessage>(om => om.Type == "ping")), Times.Exactly(2));
        }

        [Fact]
        public void DoesntPingWhenThereIsActivity()
        {
            var selfData = new SelfBotData { Id = "BOTID" };

            var mockState = new Mock<IBotState>();
            mockState.Setup(m => m.Connected).Returns(true).Verifiable();

            var mockService = new Mock<IBotServices>();
            mockService.Setup(m => m.SendRawMessage(It.Is<OutputMessage>(om => om.Type == "ping")))
                       .Verifiable();

            var handler = new PingHandler
            {
                BotState = mockState.Object,
                BotServices = mockService.Object,
                PingFrequencyMs = 200
            };

            var message = new PlainMessage
            {
                Type = EventType.Message,
                Subtype = MessageSubType.PlainMessage,
                Text = "<@BOTID>: pingdom"
            };

            Thread.Sleep(250);

            foreach (var loop in Enumerable.Range(1, 6))
            {
                handler.CanHandle(message);
                Thread.Sleep(100);
            }

            mockService.Verify(m => m.SendRawMessage(It.Is<OutputMessage>(om => om.Type == "ping")), Times.Exactly(1));
        }
    }
}
