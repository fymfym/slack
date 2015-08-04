using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model.Responses;

namespace Tab.Slack.WebApi
{
    public interface IChannelsApi
    {
        ResponseBase Archive(string channelId);
        ChannelResponse Create(string channelName);
        MessagesResponse History(string channelId, string latestTs = null,
            string oldestTs = null, bool isInclusive = false, int messageCount = 100);
        ChannelResponse Info(string channelId);
        ChannelResponse Invite(string channelId, string userId);
        ChannelResponse Join(string channelName);
        ResponseBase Kick(string channelId, string userId);
        LeaveChannelResponse Leave(string channelId);
        ChannelsResponse List(bool excludeArchived = false);
        ResponseBase Mark(string channelId, string timestamp);
        ChannelResponse Rename(string channelId, string channelName);
        PurposeResponse SetPurpose(string channelId, string channelPurpose);
        TopicResponse SetTopic(string channelId, string channelTopic);
        ResponseBase Unarchive(string channelId);
    }
}
