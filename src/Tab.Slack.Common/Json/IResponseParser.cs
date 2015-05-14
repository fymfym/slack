using Tab.Slack.Common.Model.Events;

namespace Tab.Slack.Common.Json
{
    public interface IResponseParser
    {
        EventMessageBase DeserializeEvent(string content);
        T DeserializeEvent<T>(string content) where T : EventMessageBase;
        string SerializeMessage(object message);
    }
}