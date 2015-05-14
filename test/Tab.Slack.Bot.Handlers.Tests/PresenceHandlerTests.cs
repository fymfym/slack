using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Bot;
using Tab.Slack.Bot.Handlers;
using Tab.Slack.Bot.Integration;
using Tab.Slack.Common.Model;
using Tab.Slack.Common.Model.Events;
using Tab.Slack.Common.Model.Events.Messages;
using Xunit;

namespace Tab.Slack.Bot.Handlers.Tests
{
    public class PresenceHandlerTests
    {
        [Fact]
        public void CanHandlePresenceMessages()
        {
            var message = new PresenceChange { Type = EventType.PresenceChange };
            var handler = new PresenceHandler();

            var result = handler.CanHandle(message);

            Assert.True(result);
        }

        [Fact]
        public void CanNotHandleInvalidPresenceMessages()
        {
            var message = new MeMessage { Type = EventType.Message };
            var handler = new PresenceHandler();

            var result = handler.CanHandle(message);

            Assert.False(result);
        }

        [Fact]
        public async void HandlesValidPresenceChange()
        {
            var users = new List<User>
            {
                new User { Id = "BADID", Presence = "away" },
                new User { Id = "USERID", Presence = "away" }
            };

            var mockState = new Mock<IBotState>();
            mockState.Setup(s => s.Users)
                     .Returns(users)
                     .Verifiable();

            var message = new PresenceChange
            {
                Type = EventType.Message,
                User = "USERID",
                Presence = "active"
            };

            var handler = new PresenceHandler();
            handler.BotState = mockState.Object;

            var result = await handler.HandleMessageAsync(message);

            mockState.Verify();
            Assert.Equal("away", users[0].Presence);
            Assert.Equal("active", users[1].Presence);
        }

        [Fact]
        public async void IgnoresNonMatchedUser()
        {
            var users = new List<User>
            {
                new User { Id = "BADID1", Presence = "away" },
                new User { Id = "BADID2", Presence = "away" }
            };

            var mockState = new Mock<IBotState>();
            mockState.Setup(s => s.Users)
                     .Returns(users)
                     .Verifiable();

            var message = new PresenceChange
            {
                Type = EventType.Message,
                User = "USERID",
                Presence = "active"
            };

            var handler = new PresenceHandler();
            handler.BotState = mockState.Object;

            var result = await handler.HandleMessageAsync(message);

            mockState.Verify();
            Assert.Equal("away", users[0].Presence);
            Assert.Equal("away", users[1].Presence);
        }
    }
}
