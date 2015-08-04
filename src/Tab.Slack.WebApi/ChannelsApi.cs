using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model.Responses;

namespace Tab.Slack.WebApi
{
    public class ChannelsApi : IChannelsApi
    {
        private readonly IRequestHandler request;

        public ChannelsApi(IRequestHandler requestHandler)
        {
            if (requestHandler == null)
                throw new ArgumentNullException(nameof(requestHandler));

            this.request = requestHandler;
        }

        public ResponseBase Archive(string channelId)
        {
            var apiPath = this.request.BuildApiPath("/channels.archive", channel => channelId);
            var response = this.request.ExecuteAndDeserializeRequest<ResponseBase>(apiPath);

            return response;
        }

        public ChannelResponse Create(string channelName)
        {
            var apiPath = this.request.BuildApiPath("/channels.create", name => channelName);
            var response = this.request.ExecuteAndDeserializeRequest<ChannelResponse>(apiPath);

            return response;
        }

        public MessagesResponse History(string channelId, string latestTs = null,
            string oldestTs = null, bool isInclusive = false, int messageCount = 100)
        {
            var apiPath = this.request.BuildApiPath("/channels.history",
                                        channel => channelId,
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

        public ChannelResponse Info(string channelId)
        {
            var apiPath = this.request.BuildApiPath("/channels.info", channel => channelId);
            var response = this.request.ExecuteAndDeserializeRequest<ChannelResponse>(apiPath);

            return response;
        }

        public ChannelResponse Invite(string channelId, string userId)
        {
            var apiPath = this.request.BuildApiPath("/channels.invite", channel => channelId, user => userId);
            var response = this.request.ExecuteAndDeserializeRequest<ChannelResponse>(apiPath);

            return response;
        }

        public ChannelResponse Join(string channelName)
        {
            var apiPath = this.request.BuildApiPath("/channels.join", name => channelName);
            var response = this.request.ExecuteAndDeserializeRequest<ChannelResponse>(apiPath);

            return response;
        }

        public ResponseBase Kick(string channelId, string userId)
        {
            var apiPath = this.request.BuildApiPath("/channels.kick", channel => channelId, user => userId);
            var response = this.request.ExecuteAndDeserializeRequest<ResponseBase>(apiPath);

            return response;
        }

        public LeaveChannelResponse Leave(string channelId)
        {
            var apiPath = this.request.BuildApiPath("/channels.leave", channel => channelId);
            var response = this.request.ExecuteAndDeserializeRequest<LeaveChannelResponse>(apiPath);

            return response;
        }

        public ChannelsResponse List(bool excludeArchived = false)
        {
            var apiPath = this.request.BuildApiPath("/channels.list", exclude_archived => excludeArchived ? "1" : "0");
            var response = this.request.ExecuteAndDeserializeRequest<ChannelsResponse>(apiPath);

            return response;
        }

        public ResponseBase Mark(string channelId, string timestamp)
        {
            var apiPath = this.request.BuildApiPath("/channels.mark", channel => channelId, ts => timestamp);
            var response = this.request.ExecuteAndDeserializeRequest<ResponseBase>(apiPath);

            return response;
        }

        public ChannelResponse Rename(string channelId, string channelName)
        {
            var apiPath = this.request.BuildApiPath("/channels.rename", channel => channelId, name => channelName);
            var response = this.request.ExecuteAndDeserializeRequest<ChannelResponse>(apiPath);

            return response;
        }

        public PurposeResponse SetPurpose(string channelId, string channelPurpose)
        {
            var apiPath = this.request.BuildApiPath("/channels.setPurpose", channel => channelId, purpose => channelPurpose);
            var response = this.request.ExecuteAndDeserializeRequest<PurposeResponse>(apiPath);

            return response;
        }

        public TopicResponse SetTopic(string channelId, string channelTopic)
        {
            var apiPath = this.request.BuildApiPath("/channels.setTopic", channel => channelId, topic => channelTopic);
            var response = this.request.ExecuteAndDeserializeRequest<TopicResponse>(apiPath);

            return response;
        }

        public ResponseBase Unarchive(string channelId)
        {
            var apiPath = this.request.BuildApiPath("/channels.unarchive", channel => channelId);
            var response = this.request.ExecuteAndDeserializeRequest<ResponseBase>(apiPath);

            return response;
        }
    }
}
