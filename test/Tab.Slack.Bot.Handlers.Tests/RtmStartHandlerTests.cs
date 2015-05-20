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
    public class RtmStartHandlerTests
    {
        [Fact]
        public void CanHandleRtmStart()
        {
            var message = new RtmStartResponse { Type = EventType.RtmStart };
            var handler = new RtmStartHandler();

            var result = handler.CanHandle(message);

            Assert.True(result);
        }

        [Fact]
        public void CanNotHandleInvalidRtmStart()
        {
            var message = new MeMessage { Type = EventType.Message };
            var handler = new RtmStartHandler();

            var result = handler.CanHandle(message);

            Assert.False(result);
        }

        // TODO: temp implementation for now, will TDD new approach
    }
}
