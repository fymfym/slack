using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model.Requests;
using Tab.Slack.Common.Model.Responses;

namespace Tab.Slack.WebApi
{
    public class GroupsApi : IGroupsApi
    {
        private readonly IRequestHandler request;

        public GroupsApi(IRequestHandler requestHandler)
        {
            if (requestHandler == null)
                throw new ArgumentNullException(nameof(requestHandler));

            this.request = requestHandler;
        }

        public ResponseBase Archive(string groupId)
        {
            var apiPath = this.request.BuildApiPath("/groups.archive", channel => groupId);
            var response = this.request.ExecuteAndDeserializeRequest<ResponseBase>(apiPath);

            return response;
        }

        public CloseResponse Close(string groupId)
        {
            var apiPath = this.request.BuildApiPath("/groups.close", channel => groupId);
            var response = this.request.ExecuteAndDeserializeRequest<CloseResponse>(apiPath);

            return response;
        }

        public GroupResponse Create(string groupName)
        {
            var apiPath = this.request.BuildApiPath("/groups.create", name => groupName);
            var response = this.request.ExecuteAndDeserializeRequest<GroupResponse>(apiPath);

            return response;
        }

        public GroupResponse CreateChild(string groupId)
        {
            var apiPath = this.request.BuildApiPath("/groups.createChild", channel => groupId);
            var response = this.request.ExecuteAndDeserializeRequest<GroupResponse>(apiPath);

            return response;
        }

        public MessagesResponse History(string groupId, string latestTs = null,
            string oldestTs = null, bool isInclusive = false, int messageCount = 100)
        {
            var apiPath = this.request.BuildApiPath("/groups.history",
                                        channel => groupId,
                                        latest => latestTs,
                                        oldest => oldestTs,
                                        inclusive => isInclusive ? "1" : "0",
                                        count => messageCount.ToString());

            var response = this.request.ExecuteAndDeserializeRequest<MessagesResponse>(apiPath);

            if (response.Messages != null)
            {
                response.Messages = this.request.ResponseParser.RemapMessagesToConcreteTypes(response.Messages)
                                                       .ToList();
            }

            return response;
        }

        public GroupResponse Info(string groupId)
        {
            var apiPath = this.request.BuildApiPath("/groups.info", channel => groupId);
            var response = this.request.ExecuteAndDeserializeRequest<GroupResponse>(apiPath);

            return response;
        }

        public GroupResponse Invite(string groupId, string userId)
        {
            var apiPath = this.request.BuildApiPath("/groups.invite", channel => groupId, user => userId);
            var response = this.request.ExecuteAndDeserializeRequest<GroupResponse>(apiPath);

            return response;
        }

        public ResponseBase Kick(string groupId, string userId)
        {
            var apiPath = this.request.BuildApiPath("/groups.kick", channel => groupId, user => userId);
            var response = this.request.ExecuteAndDeserializeRequest<ResponseBase>(apiPath);

            return response;
        }

        public ResponseBase Leave(string groupId)
        {
            var apiPath = this.request.BuildApiPath("/groups.leave", channel => groupId);
            var response = this.request.ExecuteAndDeserializeRequest<ResponseBase>(apiPath);

            return response;
        }

        public GroupsResponse List(bool excludeArchived = false)
        {
            var apiPath = this.request.BuildApiPath("/groups.list", exclude_archived => excludeArchived ? "1" : "0");
            var response = this.request.ExecuteAndDeserializeRequest<GroupsResponse>(apiPath);

            return response;
        }

        public ResponseBase Mark(string groupId, string timestamp)
        {
            var apiPath = this.request.BuildApiPath("/groups.mark", channel => groupId, ts => timestamp);
            var response = this.request.ExecuteAndDeserializeRequest<ResponseBase>(apiPath);

            return response;
        }

        public ResponseBase Open(string groupId)
        {
            var apiPath = this.request.BuildApiPath("/groups.open", channel => groupId);
            var response = this.request.ExecuteAndDeserializeRequest<ResponseBase>(apiPath);

            return response;
        }

        public ChannelResponse Rename(string groupId, string groupName)
        {
            var apiPath = this.request.BuildApiPath("/groups.rename", channel => groupId, name => groupName);
            var response = this.request.ExecuteAndDeserializeRequest<ChannelResponse>(apiPath);

            return response;
        }

        public PurposeResponse SetPurpose(string groupId, string groupPurpose)
        {
            var apiPath = this.request.BuildApiPath("/groups.setPurpose", channel => groupId, purpose => groupPurpose);
            var response = this.request.ExecuteAndDeserializeRequest<PurposeResponse>(apiPath);

            return response;
        }

        public TopicResponse SetTopic(string groupId, string groupTopic)
        {
            var apiPath = this.request.BuildApiPath("/groups.setTopic", channel => groupId, topic => groupTopic);
            var response = this.request.ExecuteAndDeserializeRequest<TopicResponse>(apiPath);

            return response;
        }

        public ResponseBase Unarchive(string groupId)
        {
            var apiPath = this.request.BuildApiPath("/groups.unarchive", channel => groupId);
            var response = this.request.ExecuteAndDeserializeRequest<ResponseBase>(apiPath);

            return response;
        }
    }
}
