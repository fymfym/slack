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
    public class GroupHandlerTests
    {
        [Fact]
        public void CanHandleGroupMessages()
        {
            var handler = new GroupHandler();

            bool result = false;
            var messageTestCases = new List<EventMessageBase>
            {
                new GroupArchive { Type = EventType.GroupArchive },
                new GroupJoined { Type = EventType.GroupJoined },
                new GroupLeft { Type = EventType.GroupLeft },
                new GroupMarked { Type = EventType.GroupMarked },
                new GroupRename { Type = EventType.GroupRename },
                new GroupUnarchive { Type = EventType.GroupUnarchive }
            };

            foreach (var messageTestCase in messageTestCases)
            {
                result = handler.CanHandle(messageTestCase);
                Assert.True(result);
            }
        }

        [Fact]
        public void HandlesValidGroupArchive()
        {
            var groups = new List<Group>
            {
                new Group { Id = "CHANID1", IsArchived = false },
                new Group { Id = "CHANID2", IsArchived = false }
            };

            var mockState = SetupGroupsMock(groups);

            var message = new GroupArchive
            {
                Type = EventType.GroupArchive,
                Channel = "CHANID1"
            };

            RunHandler(message, mockState);

            Assert.Equal(2, groups.Count);
            Assert.Equal(true, groups.First(c => c.Id == "CHANID1").IsArchived);
            Assert.Equal(false, groups.First(c => c.Id == "CHANID2").IsArchived);
        }
        
        [Fact]
        public void HandlesValidGroupJoined()
        {
            var groups = new List<Group>
            {
                new Group { Id = "CHANID1", IsOpen = false },
                new Group { Id = "CHANID2", IsOpen = true }
            };

            var mockState = SetupGroupsMock(groups);

            var message = new GroupJoined
            {
                Type = EventType.GroupJoined,
                Channel = new Group {  Id = "CHANID1", IsOpen = true }
            };

            RunHandler(message, mockState);

            Assert.True(groups.All(c => c.IsOpen));
        }

        [Fact]
        public void HandlesValidGroupLeft()
        {
            var groups = new List<Group>
            {
                new Group { Id = "CHANID1", IsOpen = false },
                new Group { Id = "CHANID2", IsOpen = true }
            };

            var mockState = SetupGroupsMock(groups);

            var message = new GroupLeft
            {
                Type = EventType.GroupLeft,
                Channel = new Group { Id = "CHANID2" }
            };

            RunHandler(message, mockState);

            Assert.True(groups.All(c => c.IsOpen == false));
        }

        [Fact]
        public void HandlesValidGroupMarked()
        {
            var groups = new List<Group>
            {
                new Group { Id = "CHANID1" },
                new Group { Id = "CHANID2", LastRead = "1000" }
            };

            var mockState = SetupGroupsMock(groups);

            var message = new GroupMarked
            {
                Type = EventType.GroupMarked,
                Channel = "CHANID2",
                Ts = "1001"
            };

            RunHandler(message, mockState);

            Assert.Equal(2, groups.Count);
            Assert.Equal("1001", groups.First(c => c.Id == "CHANID2").LastRead);
        }

        [Fact]
        public void HandlesValidGroupUnarchive()
        {
            var groups = new List<Group>
            {
                new Group { Id = "CHANID1", IsArchived = true },
                new Group { Id = "CHANID2", IsArchived = false }
            };

            var mockState = SetupGroupsMock(groups);

            var message = new GroupUnarchive
            {
                Type = EventType.GroupUnarchive,
                Channel = "CHANID1"
            };

            RunHandler(message, mockState);

            Assert.Equal(2, groups.Count);
            Assert.Equal(false, groups.First(c => c.Id == "CHANID1").IsArchived);
            Assert.Equal(false, groups.First(c => c.Id == "CHANID2").IsArchived);
        }

        private async void RunHandler(EventMessageBase message, Mock<IBotState> mockState) 
        {
            var handler = new GroupHandler();
            handler.BotState = mockState.Object;

            var result = await handler.HandleMessageAsync(message);

            mockState.Verify();
        }

        private Mock<IBotState> SetupGroupsMock(List<Group> groups)
        {
            var mockState = new Mock<IBotState>();
            mockState.Setup(s => s.Groups)
                     .Returns(groups)
                     .Verifiable();

            return mockState;
        }
    }
}
