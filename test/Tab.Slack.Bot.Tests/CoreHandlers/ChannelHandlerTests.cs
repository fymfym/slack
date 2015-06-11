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
    public class ChannelHandlerTests
    {
        [Fact]
        public void CanHandleChannelMessages()
        {
            var handler = new ChannelHandler();

            bool result = false;
            var messageTestCases = new List<EventMessageBase>
            {
                new ChannelArchive { Type = EventType.ChannelArchive },
                new ChannelCreated { Type = EventType.ChannelCreated },
                new ChannelDeleted { Type = EventType.ChannelDeleted },
                new ChannelJoined { Type = EventType.ChannelJoined },
                new ChannelLeft { Type = EventType.ChannelLeft },
                new ChannelMarked { Type = EventType.ChannelMarked },
                new ChannelRename { Type = EventType.ChannelRename },
                new ChannelUnarchive { Type = EventType.ChannelUnarchive },
            };

            foreach (var messageTestCase in messageTestCases)
            {
                result = handler.CanHandle(messageTestCase);
                Assert.True(result);
            }
        }

        [Fact]
        public void HandlesValidChannelArchive()
        {
            var channels = new List<Channel>
            {
                new Channel { Id = "CHANID1", IsArchived = false },
                new Channel { Id = "CHANID2", IsArchived = false }
            };

            var mockState = SetupChannelsMock(channels);

            var message = new ChannelArchive
            {
                Type = EventType.ChannelArchive,
                Channel = "CHANID1"
            };

            RunHandler(message, mockState);

            Assert.Equal(2, channels.Count);
            Assert.Equal(true, channels.First(c => c.Id == "CHANID1").IsArchived);
            Assert.Equal(false, channels.First(c => c.Id == "CHANID2").IsArchived);
        }

        [Fact]
        public void HandlesValidChannelCreated()
        {
            var channels = new List<Channel>
            {
                new Channel { Id = "CHANID1", IsArchived = false },
            };

            var mockState = SetupChannelsMock(channels);

            var message = new ChannelCreated
            {
                Type = EventType.ChannelCreated,
                Channel = new Channel { Id = "CHANID2" }
            };

            RunHandler(message, mockState);

            Assert.Equal(2, channels.Count);
            Assert.True(channels.Any(c => c.Id == "CHANID1"));
            Assert.True(channels.Any(c => c.Id == "CHANID2"));
        }

        [Fact]
        public void HandlesValidChannelDeleted()
        {
            var channels = new List<Channel>
            {
                new Channel { Id = "CHANID1", IsArchived = false },
                new Channel { Id = "CHANID2", IsArchived = false }
            };

            var mockState = SetupChannelsMock(channels);

            var message = new ChannelDeleted
            {
                Type = EventType.ChannelDeleted,
                Channel = "CHANID1"
            };

            RunHandler(message, mockState);

            Assert.Equal(1, channels.Count);
            Assert.Equal("CHANID2", channels[0].Id);
        }

        [Fact]
        public void HandlesValidChannelJoined()
        {
            var channels = new List<Channel>
            {
                new Channel { Id = "CHANID1", IsMember = false },
                new Channel { Id = "CHANID2", IsMember = true }
            };

            var mockState = SetupChannelsMock(channels);

            var message = new ChannelJoined
            {
                Type = EventType.ChannelJoined,
                Channel = new Channel {  Id = "CHANID1", IsMember = true }
            };

            RunHandler(message, mockState);

            Assert.True(channels.All(c => c.IsMember));
        }

        [Fact]
        public void HandlesValidChannelLeft()
        {
            var channels = new List<Channel>
            {
                new Channel { Id = "CHANID1", IsMember = false },
                new Channel { Id = "CHANID2", IsMember = true }
            };

            var mockState = SetupChannelsMock(channels);

            var message = new ChannelLeft
            {
                Type = EventType.ChannelLeft,
                Channel = "CHANID2"
            };

            RunHandler(message, mockState);

            Assert.True(channels.All(c => c.IsMember == false));
        }

        [Fact]
        public void HandlesValidChannelMarked()
        {
            var channels = new List<Channel>
            {
                new Channel { Id = "CHANID1" },
                new Channel { Id = "CHANID2", LastRead = "1000" }
            };

            var mockState = SetupChannelsMock(channels);

            var message = new ChannelMarked
            {
                Type = EventType.ChannelMarked,
                Channel = "CHANID2",
                Ts = "1001"
            };

            RunHandler(message, mockState);

            Assert.Equal(2, channels.Count);
            Assert.Equal("1001", channels.First(c => c.Id == "CHANID2").LastRead);
        }

        [Fact]
        public void HandlesValidChannelUnarchive()
        {
            var channels = new List<Channel>
            {
                new Channel { Id = "CHANID1", IsArchived = true },
                new Channel { Id = "CHANID2", IsArchived = false }
            };

            var mockState = SetupChannelsMock(channels);

            var message = new ChannelUnarchive
            {
                Type = EventType.ChannelUnarchive,
                Channel = "CHANID1"
            };

            RunHandler(message, mockState);

            Assert.Equal(2, channels.Count);
            Assert.Equal(false, channels.First(c => c.Id == "CHANID1").IsArchived);
            Assert.Equal(false, channels.First(c => c.Id == "CHANID2").IsArchived);
        }

        private async void RunHandler(EventMessageBase message, Mock<IBotState> mockState) 
        {
            var handler = new ChannelHandler();
            handler.BotState = mockState.Object;

            var result = await handler.HandleMessageAsync(message);

            mockState.Verify();
        }

        private Mock<IBotState> SetupChannelsMock(List<Channel> channels)
        {
            var mockState = new Mock<IBotState>();
            mockState.Setup(s => s.Channels)
                     .Returns(channels)
                     .Verifiable();

            return mockState;
        }
    }
}
