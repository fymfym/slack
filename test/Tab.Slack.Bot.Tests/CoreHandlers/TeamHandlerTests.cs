using Moq;
using Newtonsoft.Json.Linq;
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
    public class TeamHandlerTests
    {
        [Fact]
        public void CanHandleTeamMessages()
        {
            var handler = new TeamHandler();

            bool result = false;
            var messageTestCases = new List<EventMessageBase>
            {
                new PrefChange { Type = EventType.PrefChange },
                new UserChange { Type = EventType.UserChange },
                new TeamJoin { Type = EventType.TeamJoin },
                new TeamPrefChange { Type = EventType.TeamPrefChange },
                new TeamRename { Type = EventType.TeamRename },
                new TeamDomainChange { Type = EventType.TeamDomainChange },
                new EmailDomainChanged { Type = EventType.EmailDomainChanged }
            };

            foreach (var messageTestCase in messageTestCases)
            {
                result = handler.CanHandle(messageTestCase);
                Assert.True(result);
            }
        }

        [Fact]
        public void HandlesValidPrefChange()
        {
            var selfBot = new SelfBotData { Prefs = new JObject() };
            selfBot.Prefs["pref1"] = "old";

            var mockState = new Mock<IBotState>();
            mockState.Setup(s => s.Self).Returns(selfBot).Verifiable();

            var message = new PrefChange
            {
                Type = EventType.PrefChange,
                Name = "pref1",
                Value = "foobar"
            };

            RunHandler(message, mockState);

            Assert.Equal("foobar", selfBot.Prefs["pref1"]);
        }

        [Fact]
        public void HandlesValidUserChange()
        {
            var users = new List<User>
            {
                new User { Id = "UID1", Name = "User1" },
                new User { Id = "UID2", Name = "User2" }
            };

            var mockState = new Mock<IBotState>();
            mockState.Setup(s => s.Users).Returns(users).Verifiable();

            var message = new UserChange
            {
                Type = EventType.UserChange,
                User = new User { Id = "UID1", Name = "User1b" }
            };

            RunHandler(message, mockState);

            Assert.Equal(2, users.Count);
            Assert.Equal("User1b", users.First(u => u.Id == "UID1").Name);
            Assert.Equal("User2", users.First(u => u.Id == "UID2").Name);
        }

        [Fact]
        public void HandlesValidTeamJoin()
        {
            var users = new List<User>
            {
                new User { Id = "UID1", Name = "User1" }
            };

            var mockState = new Mock<IBotState>();
            mockState.Setup(s => s.Users).Returns(users).Verifiable();

            var message = new TeamJoin
            {
                Type = EventType.TeamJoin,
                User = new User { Id = "UID2", Name = "User2" }
            };

            RunHandler(message, mockState);

            Assert.Equal(2, users.Count);
            Assert.Equal("User1", users.First(u => u.Id == "UID1").Name);
            Assert.Equal("User2", users.First(u => u.Id == "UID2").Name);
        }

        [Fact]
        public void HandlesValidTeamPrefChange()
        {
            var team = new TeamData { Prefs = new JObject() };
            team.Prefs["pref1"] = false;

            var mockState = new Mock<IBotState>();
            mockState.Setup(s => s.Team).Returns(team).Verifiable();

            var message = new TeamPrefChange
            {
                Type = EventType.TeamPrefChange,
                Name = "pref1",
                Value = true
            };

            RunHandler(message, mockState);

            Assert.True((bool)team.Prefs["pref1"]);
        }

        [Fact]
        public void HandlesValidTeamRename()
        {
            var team = new TeamData { Name = "Team1" };
            var mockState = new Mock<IBotState>();
            mockState.Setup(s => s.Team).Returns(team).Verifiable();

            var message = new TeamRename
            {
                Type = EventType.TeamRename,
                Name = "Team1b"
            };

            RunHandler(message, mockState);

            Assert.Equal("Team1b", team.Name);
        }

        [Fact]
        public void HandlesValidTeamDomainChange()
        {
            var team = new TeamData { Domain = "team.com" };
            var mockState = new Mock<IBotState>();
            mockState.Setup(s => s.Team).Returns(team).Verifiable();

            var message = new TeamDomainChange
            {
                Type = EventType.TeamDomainChange,
                Domain = "team2.com"
            };

            RunHandler(message, mockState);

            Assert.Equal("team2.com", team.Domain);
        }

        [Fact]
        public void HandlesValidEmailDomainChanged()
        {
            var team = new TeamData { EmailDomain = "team.com" };
            var mockState = new Mock<IBotState>();
            mockState.Setup(s => s.Team).Returns(team).Verifiable();

            var message = new EmailDomainChanged
            {
                Type = EventType.EmailDomainChanged,
                EmailDomain = "team2.com"
            };

            RunHandler(message, mockState);

            Assert.Equal("team2.com", team.EmailDomain);
        }

        private async void RunHandler(EventMessageBase message, Mock<IBotState> mockState)
        {
            var handler = new TeamHandler();
            handler.BotState = mockState.Object;

            var result = await handler.HandleMessageAsync(message);

            mockState.Verify();
        }
    }
}
