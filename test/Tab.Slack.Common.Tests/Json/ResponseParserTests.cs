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

        [Fact]
        public void ShouldRemapMessagesToConcreteTypes()
        {
            var meMessage = new MeMessage
            {
                Type = EventType.Message,
                Subtype = MessageSubType.MeMessage,
                Text = "me",
                Team = "team1"
            };

            var topicMessage = new ChannelTopic
            {
                Type = EventType.Message,
                Subtype = MessageSubType.ChannelTopic,
                Topic = "topic"
            };

            var plainMessage = new PlainMessage
            {
                Type = EventType.Message,
                Subtype = MessageSubType.PlainMessage,
                Text = "text"
            };

            var parser = new ResponseParser();

            var messages = new List<MessageBase>
            {
                parser.Deserialize<MessageBase>(parser.SerializeMessage(meMessage)),
                parser.Deserialize<MessageBase>(parser.SerializeMessage(topicMessage)),
                parser.Deserialize<MessageBase>(parser.SerializeMessage(plainMessage))
            };

            var concreteMessages = parser.RemapMessagesToConcreteTypes(messages).ToList();

            Assert.IsType<MeMessage>(concreteMessages[0]);
            Assert.IsType<ChannelTopic>(concreteMessages[1]);
            Assert.IsType<PlainMessage>(concreteMessages[2]);

            Assert.Equal(EventType.Message, concreteMessages[0].Type);
            Assert.Equal(MessageSubType.MeMessage, concreteMessages[0].Subtype);
            Assert.Equal("me", concreteMessages[0].Text);
            Assert.Equal("team1", concreteMessages[0].Team);

            Assert.Equal(EventType.Message, concreteMessages[1].Type);
            Assert.Equal("topic", ((ChannelTopic)concreteMessages[1]).Topic);
        }
    }
}
