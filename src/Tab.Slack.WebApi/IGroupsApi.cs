using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model.Requests;
using Tab.Slack.Common.Model.Responses;

namespace Tab.Slack.WebApi
{
    public interface IGroupsApi
    {
        ResponseBase Archive(string groupId);
        CloseResponse Close(string groupId);
        GroupResponse Create(string groupName);
        GroupResponse CreateChild(string groupId);
        MessagesResponse History(string groupId, string latestTs = null,
            string oldestTs = null, bool isInclusive = false, int messageCount = 100);
        GroupResponse Info(string groupId);
        GroupResponse Invite(string groupId, string userId);
        ResponseBase Kick(string groupId, string userId);
        ResponseBase Leave(string groupId);
        GroupsResponse List(bool excludeArchived = false);
        ResponseBase Mark(string groupId, string timestamp);
        ResponseBase Open(string groupId);
        ChannelResponse Rename(string groupId, string groupName);
        PurposeResponse SetPurpose(string groupId, string groupPurpose);
        TopicResponse SetTopic(string groupId, string groupTopic);
        ResponseBase Unarchive(string groupId);
    }
}
