using Tab.Slack.Common.Json;
using Tab.Slack.Common.Model;
using Tab.Slack.Common.Model.Requests;
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
        ChannelResponse ChannelCreate(string channelName);
        MessagesResponse ChannelHistory(string channelId, string latestTs = null,
            string oldestTs = null, bool isInclusive = false, int messageCount = 100);
        ChannelResponse ChannelInfo(string channelId);
        ChannelResponse ChannelInvite(string channelId, string userId);
        ChannelResponse ChannelJoin(string channelName);
        ResponseBase ChannelKick(string channelId, string userId);
        LeaveChannelResponse ChannelLeave(string channelId);
        ChannelsResponse ChannelsList(bool excludeArchived = false);
        ResponseBase ChannelMark(string channelId, string timestamp);
        ChannelResponse ChannelRename(string channelId, string channelName);
        PurposeResponse ChannelSetPurpose(string channelId, string channelPurpose);
        TopicResponse ChannelSetTopic(string channelId, string channelTopic);
        ResponseBase ChannelUnarchive(string channelId);
        ChatDeleteResponse ChatDelete(string channelId, string timestamp);
        MessageResponse ChatPostMessage(PostMessageRequest request);
    }
}