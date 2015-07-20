using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tab.Slack.Bot.Integration;
using Tab.Slack.Common.Model;
using Tab.Slack.Common.Model.Events;
using Tab.Slack.Common.Model.Events.Messages;
using Xunit;

namespace Tab.Slack.Bot.CoreHandlers.Tests
{
    public class UserPingHandlerTests
    {
        [Fact]
        public void CanHandlePingMessages()
        {
            var selfData = new SelfBotData { Id = "BOTID" };

            var mockState = new Mock<IBotState>();
            mockState.Setup(m => m.Ims).Returns(new List<DirectMessageChannel>()).Verifiable();
            mockState.Setup(m => m.Self).Returns(selfData).Verifiable();

            var message = new PlainMessage
            {
                Type = EventType.Message,
                Subtype = MessageSubType.PlainMessage,
                Text = "<@BOTID>: ping"
            };

            var handler = new UserPingHandler();
            handler.BotState = mockState.Object;

            var result = handler.CanHandle(message);

            Assert.True(result);
        }

        [Fact]
        public void CanNotHandleInvalidPingMessages()
        {
            var selfData = new SelfBotData { Id = "BOTID" };

            var mockState = new Mock<IBotState>();
            mockState.Setup(m => m.Ims).Returns(new List<DirectMessageChannel>()).Verifiable();
            mockState.Setup(m => m.Self).Returns(selfData).Verifiable();

            var message = new PlainMessage
            {
                Type = EventType.Message,
                Subtype = MessageSubType.PlainMessage,
                Text = "<@BOTID>: pingdom"
            };

            var handler = new UserPingHandler();
            handler.BotState = mockState.Object;

            var result = handler.CanHandle(message);

            Assert.False(result);
        }

        [Fact]
        public void CanHandleImPingMessages()
        {
            var ims = new List<DirectMessageChannel>
            {
                new DirectMessageChannel { Id = "IMID" }
            };

            var mockState = new Mock<IBotState>();
            mockState.Setup(m => m.Ims).Returns(ims).Verifiable();

            var message = new PlainMessage
            {
                Type = EventType.Message,
                Subtype = MessageSubType.PlainMessage,
                Channel = "IMID",
                Text = "ping"
            };

            var handler = new UserPingHandler();
            handler.BotState = mockState.Object;

            var result = handler.CanHandle(message);

            Assert.True(result);
        }

        [Fact]
        public async void HandlesChannelMessage()
        {
            var selfData = new SelfBotData { Id = "BOTID" };

            var mockState = new Mock<IBotState>();
            mockState.Setup(m => m.Ims).Returns(new List<DirectMessageChannel>()).Verifiable();

            string channelResult = "";
            string messageResult = "";

            var mockService = new Mock<IBotServices>();
            mockService.Setup(s => s.SendMessage(It.IsAny<string>(), It.IsAny<string>()))
                       .Callback<string, string>((c, m) => 
                       {
                           channelResult = c;
                           messageResult = m;
                       })
                       .Returns(1)
                       .Verifiable();

            var message = new PlainMessage
            {
                Type = EventType.Message,
                Subtype = MessageSubType.PlainMessage,
                Text = "<@BOTID>: ping",
                Channel = "CHANID",
                User = "USERID"
            };

            var handler = new UserPingHandler();
            handler.BotState = mockState.Object;
            handler.BotServices = mockService.Object;

            var result = await handler.HandleMessageAsync(message);

            mockState.Verify();
            mockService.Verify();
            Assert.Equal(ProcessingChainResult.Continue, result);
            Assert.Equal("CHANID", channelResult);
            Assert.Equal("<@USERID>: pong", messageResult);
        }

        [Fact]
        public async void HandlesImMessage()
        {
            var ims = new List<DirectMessageChannel>()
            {
                new DirectMessageChannel { Id = "TESTID" }
            };

            var mockState = new Mock<IBotState>();
            mockState.Setup(m => m.Ims).Returns(ims).Verifiable();

            string channelResult = "";
            string messageResult = "";

            var mockService = new Mock<IBotServices>();
            mockService.Setup(s => s.SendMessage(It.IsAny<string>(), It.IsAny<string>()))
                       .Callback<string, string>((c, m) =>
                       {
                           channelResult = c;
                           messageResult = m;
                       })
                       .Returns(1)
                       .Verifiable();

            var message = new PlainMessage
            {
                Type = EventType.Message,
                Subtype = MessageSubType.PlainMessage,
                Text = "ping",
                Channel = "TESTID",
                User = "USERID"
            };

            var handler = new UserPingHandler();
            handler.BotState = mockState.Object;
            handler.BotServices = mockService.Object;

            var result = await handler.HandleMessageAsync(message);

            mockState.Verify();
            mockService.Verify();
            Assert.Equal(ProcessingChainResult.Continue, result);
            Assert.Equal("TESTID", channelResult);
            Assert.Equal("pong", messageResult);
        }
    }
}
