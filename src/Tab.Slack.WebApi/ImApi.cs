using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model.Requests;
using Tab.Slack.Common.Model.Responses;

namespace Tab.Slack.WebApi
{
    public class ImApi : IImApi
    {
        private readonly IRequestHandler request;

        public ImApi(IRequestHandler requestHandler)
        {
            if (requestHandler == null)
                throw new ArgumentNullException(nameof(requestHandler));

            this.request = requestHandler;
        }

        public CloseResponse Close(string imId)
        {
            var apiPath = this.request.BuildApiPath("/im.close", channel => imId);
            var response = this.request.ExecuteAndDeserializeRequest<CloseResponse>(apiPath);

            return response;
        }

        public MessagesResponse History(string imId, string latestTs = null,
            string oldestTs = null, bool isInclusive = false, int messageCount = 100)
        {
            var apiPath = this.request.BuildApiPath("/im.history",
                                        channel => imId,
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

        public ImsResponse List(bool excludeArchived = false)
        {
            var apiPath = this.request.BuildApiPath("/im.list", exclude_archived => excludeArchived ? "1" : "0");
            var response = this.request.ExecuteAndDeserializeRequest<ImsResponse>(apiPath);

            return response;
        }

        public ResponseBase Mark(string imId, string timestamp)
        {
            var apiPath = this.request.BuildApiPath("/im.mark", channel => imId, ts => timestamp);
            var response = this.request.ExecuteAndDeserializeRequest<ResponseBase>(apiPath);

            return response;
        }

        public ImOpenResponse Open(string userId)
        {
            var apiPath = this.request.BuildApiPath("/im.open", user => userId);
            var response = this.request.ExecuteAndDeserializeRequest<ImOpenResponse>(apiPath);

            return response;
        }
    }
}
