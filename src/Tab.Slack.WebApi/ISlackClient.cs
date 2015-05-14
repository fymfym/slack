using Tab.Slack.Common.Json;
using Tab.Slack.Common.Model;
using Tab.Slack.Common.Model.Responses;

namespace Tab.Slack.WebApi
{
    public interface ISlackClient
    {
        IResponseParser ResponseParser { get; set; }

        RtmStartResponse RtmStart();
        ApiTestResponse ApiTest(string error = null, params string[] args);
        AuthTestResponse AuthTest();
        ResponseBase ChannelArchive(string channelId);
        ChannelResponse ChannelCreate(string name);
        MessagesResponse ChannelHistory(string channelName, string latestTs = null,
            string oldestTs = null, bool isInclusive = false, int messageCount = 100);
        ChannelResponse ChannelInfo(string channelName);
    }
}