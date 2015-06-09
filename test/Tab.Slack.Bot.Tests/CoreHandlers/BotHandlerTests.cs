using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tab.Slack.Common.Model;
using Tab.Slack.Common.Model.Events;
using Tab.Slack.Common.Model.Events.Messages;
using Xunit;

namespace Tab.Slack.Bot.CoreHandlers.Tests
{
    public class BotHandlerTests
    {
        [Fact]
        public void CanHandleBotMessages()
        {
            var handler = new BotHandler();

            var result = handler.CanHandle(new BotChanged { Type = EventType.BotChanged });
            Assert.True(result);

            result = handler.CanHandle(new BotAdded { Type = EventType.BotAdded });
            Assert.True(result);
        }

        [Fact]
        public async void HandlesValidBotAdded()
        {
            var bots = new List<BotModel>
            {
                new BotModel { Id = "BOTID1" },
                new BotModel { Id = "BOTID2" }
            };

            var mockState = new Mock<IBotState>();
            mockState.Setup(s => s.Bots)
                     .Returns(bots)
                     .Verifiable();

            var message = new BotAdded
            {
                Type = EventType.BotAdded,
                Bot = new BotModel { Id = "BOTID3" }
            };

            var handler = new BotHandler();
            handler.BotState = mockState.Object;

            var result = await handler.HandleMessageAsync(message);

            mockState.Verify();
            Assert.Equal(3, bots.Count);
            Assert.Equal("BOTID3", bots.Last().Id);
        }

        [Fact]
        public async void HandlesValidBotChanged()
        {
            var bots = new List<BotModel>
            {
                new BotModel { Id = "BOTID1", Name = "NameA" },
                new BotModel { Id = "BOTID2" }
            };

            var mockState = new Mock<IBotState>();
            mockState.Setup(s => s.Bots)
                     .Returns(bots)
                     .Verifiable();

            var message = new BotChanged
            {
                Type = EventType.BotChanged,
                Bot = new BotModel { Id = "BOTID1", Name = "NameB" }
            };

            var handler = new BotHandler();
            handler.BotState = mockState.Object;

            var result = await handler.HandleMessageAsync(message);

            mockState.Verify();
            Assert.Equal(2, bots.Count);
            Assert.Equal("NameB", bots.First(b => b.Id == "BOTID1").Name);
        }

        [Fact]
        public async void AddsMissingChangedBot()
        {
            var bots = new List<BotModel>
            {
                new BotModel { Id = "BOTID2" }
            };

            var mockState = new Mock<IBotState>();
            mockState.Setup(s => s.Bots)
                     .Returns(bots)
                     .Verifiable();

            var message = new BotChanged
            {
                Type = EventType.BotChanged,
                Bot = new BotModel { Id = "BOTID1", Name = "NameB" }
            };

            var handler = new BotHandler();
            handler.BotState = mockState.Object;

            var result = await handler.HandleMessageAsync(message);

            mockState.Verify();
            Assert.Equal(2, bots.Count);
            Assert.Equal("NameB", bots.First(b => b.Id == "BOTID1").Name);
        }
    }
}
