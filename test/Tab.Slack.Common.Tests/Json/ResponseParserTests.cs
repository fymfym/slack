using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Json;
using Tab.Slack.Common.Model;
using Tab.Slack.Common.Model.Events;
using Tab.Slack.Common.Model.Events.Messages;
using Xunit;

namespace Tab.Slack.Common.Tests.Json
{
    public class ResponseParserTests
    {
        [Fact]
        public void SerializeValidMessage()
        {
            var message = new OutputMessage(1, "CHANID", "MSG");

            var parser = new ResponseParser();
            var result = parser.SerializeMessage(message);

            var expected = @"{""id"":1,""type"":""message"",""channel"":""CHANID"",""text"":""MSG""}";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CanDeserializeHelloEvent()
        {
            var messageContent = @"{""type"":""hello""}";

            var parser = new ResponseParser();
            var result = parser.Deserialize<Hello>(messageContent);

            Assert.NotNull(result);
            Assert.Equal(EventType.Hello, result.Type);
        }

        [Fact]
        public void CanDeserializeMeMessage()
        {
            var messageContent = @"{""type"":""message"",""subtype"":""me_message""}";

            var parser = new ResponseParser();
            var result = parser.Deserialize<MeMessage>(messageContent);

            Assert.NotNull(result);
            Assert.Equal(EventType.Message, result.Type);
            Assert.Equal(MessageSubType.MeMessage, result.Subtype);
        }

        [Fact]
        public void CanDeserializeEventWithExtraData()
        {
            var messageContent = @"{""type"":""message"",""subtype"":""me_message"",""something"":""extra""}";

            var parser = new ResponseParser();
            var result = parser.Deserialize<MeMessage>(messageContent);

            Assert.NotNull(result);
            Assert.Equal(EventType.Message, result.Type);
            Assert.Equal(MessageSubType.MeMessage, result.Subtype);
            Assert.Equal("extra", result.UnmatchedAdditionalJsonData["something"]);
        }

        [Fact]
        public void CanImplicitlyDeserializeHelloMessage()
        {
            var messageContent = @"{""type"":""hello""}";

            var parser = new ResponseParser();
            var result = parser.DeserializeEvent(messageContent) as Hello;

            Assert.NotNull(result);
            Assert.Equal(EventType.Hello, result.Type);
        }

        [Fact]
        public void CanImplicitlyDeserializeMeMessage()
        {
            var messageContent = @"{""type"":""message"",""subtype"":""me_message""}";

            var parser = new ResponseParser();
            var result = parser.DeserializeEvent(messageContent) as MeMessage;

            Assert.NotNull(result);
            Assert.Equal(EventType.Message, result.Type);
            Assert.Equal(MessageSubType.MeMessage, result.Subtype);
        }
    }
}
