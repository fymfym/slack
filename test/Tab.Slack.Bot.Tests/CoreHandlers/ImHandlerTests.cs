using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tab.Slack.Bot.Integration;
using Tab.Slack.Common.Model;
using Tab.Slack.Common.Model.Events;
using Xunit;

namespace Tab.Slack.Bot.CoreHandlers.Tests
{
    public class ImHandlerTests
    {
        [Fact]
        public void CanHandleImMessages()
        {
            var handler = new ImHandler();

            bool result = false;
            var messageTestCases = new List<EventMessageBase>
            {
                new ImClose { Type = EventType.ImClose },
                new ImCreated { Type = EventType.ImCreated },
                new ImMarked { Type = EventType.ImMarked }
            };

            foreach (var messageTestCase in messageTestCases)
            {
                result = handler.CanHandle(messageTestCase);
                Assert.True(result);
            }
        }

        [Fact]
        public void HandlesValidImClose()
        {
            var ims = new List<DirectMessageChannel>
            {
                new DirectMessageChannel { Id = "CHANID1" },
                new DirectMessageChannel { Id = "CHANID2" }
            };

            var mockState = SetupChannelsMock(ims);

            var message = new ImClose
            {
                Type = EventType.ImClose,
                Channel = "CHANID1"
            };

            RunHandler(message, mockState);

            Assert.Equal(1, ims.Count);
            Assert.Equal("CHANID2", ims[0].Id);
        }

        [Fact]
        public void HandlesValidImCreated()
        {
            var ims = new List<DirectMessageChannel>
            {
                new DirectMessageChannel { Id = "CHANID1" }
            };

            var mockState = SetupChannelsMock(ims);

            var message = new ImCreated
            {
                Type = EventType.ImCreated,
                Channel = new DirectMessageChannel { Id = "CHANID2" }
            };

            RunHandler(message, mockState);

            Assert.Equal(2, ims.Count);
            Assert.Equal("CHANID2", ims.Last().Id);
        }

        [Fact]
        public void HandlesValidImMarked()
        {
            var ims = new List<DirectMessageChannel>
            {
                new DirectMessageChannel { Id = "CHANID1", LastRead = "1000" }
            };

            var mockState = SetupChannelsMock(ims);

            var message = new ImMarked
            {
                Type = EventType.ImMarked,
                Channel = "CHANID1",
                Ts = "1001"
            };

            RunHandler(message, mockState);

            Assert.Equal(1, ims.Count);
            Assert.Equal("1001", ims[0].LastRead);
        }

        private async void RunHandler(EventMessageBase message, Mock<IBotState> mockState) 
        {
            var handler = new ImHandler();
            handler.BotState = mockState.Object;

            var result = await handler.HandleMessageAsync(message);

            mockState.Verify();
        }

        private Mock<IBotState> SetupChannelsMock(List<DirectMessageChannel> ims)
        {
            var mockState = new Mock<IBotState>();
            mockState.Setup(s => s.Ims)
                     .Returns(ims)
                     .Verifiable();

            return mockState;
        }
    }
}
