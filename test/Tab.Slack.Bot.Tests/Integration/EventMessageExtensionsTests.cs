using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Bot.Integration;
using Tab.Slack.Common.Model;
using Tab.Slack.Common.Model.Events;
using Tab.Slack.Common.Model.Events.Messages;
using Xunit;

namespace Tab.Slack.Bot.Tests.Integration
{
    public class EventMessageExtensionsTests
    {
        [Fact]
        public void IsOneOfShouldMatchValidList()
        {
            var message = new PlainMessage() { Type = EventType.Hello };

            var matches = new[]
            {
                EventType.AccountsChanged,
                EventType.ChannelCreated,
                EventType.ChannelLeft,
                EventType.CommandsChanged,
                EventType.Hello
            };

            var result = EventMessageExtensions.IsOneOf(message, matches);
            Assert.True(result);

            result = EventMessageExtensions.IsOneOf(message, EventType.Hello);
            Assert.True(result);
        }

        [Fact]
        public void IsOneOfShouldNotMatchInvalidList()
        {
            var message = new PlainMessage() { Type = EventType.ChannelCreated };

            var matches = new[]
            {
                EventType.ImClose,
                EventType.PinAdded,
                EventType.ChannelLeft,
                EventType.CommandsChanged,
                EventType.ManualPresenceChange
            };

            var result = EventMessageExtensions.IsOneOf(message, matches);
            Assert.False(result);
        }

        [Fact]
        public void ShouldCastToType()
        {
            EventMessageBase message = new PlainMessage();
            var result = EventMessageExtensions.CastTo<PlainMessage>(message);

            Assert.Same(message, result);
        }

        [Fact]
        public void ShouldNotCastToInvalidType()
        {
            EventMessageBase message = new PlainMessage();
            Assert.Throws<InvalidCastException>(() => EventMessageExtensions.CastTo<Hello>(message));
        }

        [Fact]
        public void IsActivePlainMessageShouldMatch()
        {
            var message = new PlainMessage()
            {
                Type = EventType.Message,
                Subtype = MessageSubType.PlainMessage,
            };

            var result = EventMessageExtensions.IsActivePlainMessage(message);

            Assert.True(result);
        }

        [Fact]
        public void IsActivePlainMessageShouldNotMatchWrongEventType()
        {
            var message = new PlainMessage()
            {
                Type = EventType.Hello,
                Subtype = MessageSubType.PlainMessage,
            };

            var result = EventMessageExtensions.IsActivePlainMessage(message);

            Assert.False(result);
        }

        [Fact]
        public void IsActivePlainMessageShouldNotMatchWrongSubType()
        {
            var message = new PlainMessage()
            {
                Type = EventType.Message,
                Subtype = MessageSubType.MeMessage,
            };

            var result = EventMessageExtensions.IsActivePlainMessage(message);

            Assert.False(result);
        }

        [Fact]
        public void IsActivePlainMessageShouldNotMatchHistoric()
        {
            var message = new PlainMessage()
            {
                Type = EventType.Message,
                Subtype = MessageSubType.PlainMessage,
                Historic = true
            };

            var result = EventMessageExtensions.IsActivePlainMessage(message);

            Assert.False(result);
        }

        [Fact]
        public void IsImShouldMatchDirectMessage()
        {
            var ims = new List<DirectMessageChannel>()
            {
                new DirectMessageChannel { Id = "BADID1" },
                new DirectMessageChannel { Id = "BADID2" },
                new DirectMessageChannel { Id = "TESTID" },
                new DirectMessageChannel { Id = "BADID3" }
            };

            var mockState = new Mock<IBotState>();
            mockState.Setup(m => m.Ims).Returns(ims).Verifiable();

            var message = new PlainMessage()
            {
                Type = EventType.Message,
                Subtype = MessageSubType.PlainMessage,
                Channel = "TESTID"
            };

            var result = EventMessageExtensions.IsIm(message, mockState.Object);

            mockState.Verify();
            Assert.True(result);
        }

        [Fact]
        public void IsImShouldNotMatchMissingDirectMessage()
        {
            var ims = new List<DirectMessageChannel>()
            {
                new DirectMessageChannel { Id = "BADID1" },
                new DirectMessageChannel { Id = "BADID2" },
                new DirectMessageChannel { Id = "BADID3" }
            };

            var mockState = new Mock<IBotState>();
            mockState.Setup(m => m.Ims).Returns(ims).Verifiable();

            var message = new PlainMessage()
            {
                Type = EventType.Message,
                Subtype = MessageSubType.PlainMessage,
                Channel = "TESTID"
            };

            var result = EventMessageExtensions.IsIm(message, mockState.Object);

            mockState.Verify();
            Assert.False(result);
        }

        [Fact]
        public void IsImShouldNotMatchNonPlainMessages()
        {
            var mockState = new Mock<IBotState>();

            var message = new MeMessage()
            {
                Type = EventType.Message,
                Subtype = MessageSubType.MeMessage,
                Channel = "TESTID"
            };

            var result = EventMessageExtensions.IsIm(message, mockState.Object);

            Assert.False(result);
        }

        [Fact]
        public void IsToMeMatchesWhenIm()
        {
            var ims = new List<DirectMessageChannel>()
            {
                new DirectMessageChannel { Id = "TESTID" }
            };

            var mockState = new Mock<IBotState>();
            mockState.Setup(m => m.Ims).Returns(ims).Verifiable();

            var message = new PlainMessage()
            {
                Type = EventType.Message,
                Subtype = MessageSubType.PlainMessage,
                Channel = "TESTID"
            };

            var result = EventMessageExtensions.IsToMe(message, mockState.Object);

            Assert.True(result);
        }

        [Fact]
        public void IsToMeMatchesWhenNamed()
        {
            var selfData = new SelfBotData { Id = "BOTID" };

            var mockState = new Mock<IBotState>();
            mockState.Setup(m => m.Ims).Returns(new List<DirectMessageChannel>()).Verifiable();
            mockState.Setup(m => m.Self).Returns(selfData).Verifiable();

            var message = new PlainMessage()
            {
                Type = EventType.Message,
                Subtype = MessageSubType.PlainMessage,
                Channel = "TESTID",
                Text = "<@BOTID>: test message"
            };

            var result = EventMessageExtensions.IsToMe(message, mockState.Object);

            mockState.Verify();
            Assert.True(result);
        }

        [Fact]
        public void IsToMeShouldNotMatchWhenNotNamed()
        {
            var selfData = new SelfBotData { Id = "BOTID" };

            var mockState = new Mock<IBotState>();
            mockState.Setup(m => m.Ims).Returns(new List<DirectMessageChannel>()).Verifiable();
            mockState.Setup(m => m.Self).Returns(selfData).Verifiable();

            var message = new PlainMessage()
            {
                Type = EventType.Message,
                Subtype = MessageSubType.PlainMessage,
                Channel = "TESTID",
                Text = "<@OTHERID>: test message"
            };

            var result = EventMessageExtensions.IsToMe(message, mockState.Object);

            mockState.Verify();
            Assert.False(result);
        }

        [Fact]
        public void ShouldMatchValidText()
        {
            var message = new PlainMessage()
            {
                Type = EventType.Message,
                Subtype = MessageSubType.PlainMessage,
                Text = "test message"
            };

            var result = EventMessageExtensions.MatchesText(message, @"\btest\b");

            Assert.True(result);
        }

        [Fact]
        public void ShouldNotMatchInvalidText()
        {
            var message = new PlainMessage()
            {
                Type = EventType.Message,
                Subtype = MessageSubType.PlainMessage,
                Text = "test message"
            };

            var result = EventMessageExtensions.MatchesText(message, @"testing");

            Assert.False(result);
        }
    }
}
