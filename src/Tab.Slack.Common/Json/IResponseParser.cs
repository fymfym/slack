using System.Collections.Generic;
using Tab.Slack.Common.Model.Events;
using Tab.Slack.Common.Model.Events.Messages;
using Tab.Slack.Common.Model.Responses;

namespace Tab.Slack.Common.Json
{
    public interface IResponseParser
    {
        EventMessageBase DeserializeEvent(string content);
        T Deserialize<T>(string content);
        string SerializeMessage(object message);
        IEnumerable<MessageBase> RemapMessagesToConcreteTypes(IEnumerable<MessageBase> messages);
        MessageBase RemapMessageToConcreteType(MessageBase baseMessage);
    }
}