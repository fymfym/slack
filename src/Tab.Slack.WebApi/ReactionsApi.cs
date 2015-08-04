using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model;
using Tab.Slack.Common.Model.Requests;
using Tab.Slack.Common.Model.Responses;

namespace Tab.Slack.WebApi
{
    public class ReactionsApi : IReactionsApi
    {
        private readonly IRequestHandler request;

        public ReactionsApi(IRequestHandler requestHandler)
        {
            if (requestHandler == null)
                throw new ArgumentNullException(nameof(requestHandler));

            this.request = requestHandler;
        }

        public ResponseBase Add(string reaction, string fileId = null, string commentId = null, string channelId = null, string ts = null)
        {
            var apiPath = this.request.BuildApiPath("/reactions.add",
                                        name => reaction,
                                        file => fileId,
                                        file_comment => commentId,
                                        channel => channelId,
                                        timestamp => ts);

            var response = this.request.ExecuteAndDeserializeRequest<ResponseBase>(apiPath);

            return response;
        }

        public ReactionItem Get(string fileId = null, string commentId = null, string channelId = null, string ts = null, bool? fullResults = null)
        {
            var apiPath = this.request.BuildApiPath("/reactions.get",
                                        file => fileId,
                                        file_comment => commentId,
                                        channel => channelId,
                                        timestamp => ts,
                                        full => fullResults);

            var response = this.request.ExecuteAndDeserializeRequest<ReactionItem>(apiPath);

            return response;
        }

        public ReactionListResponse List(string userId = null, bool? fullResults = null, int? reactionCount = null, int? pageNumber = null)
        {
            var apiPath = this.request.BuildApiPath("/reactions.list",
                                        user => userId,
                                        full => fullResults,
                                        page => pageNumber,
                                        count => reactionCount);

            var response = this.request.ExecuteAndDeserializeRequest<ReactionListResponse>(apiPath);

            return response;
        }

        public ResponseBase Remove(string reaction, string fileId = null, string commentId = null, string channelId = null, string ts = null)
        {
            var apiPath = this.request.BuildApiPath("/reactions.remove",
                                        name => reaction,
                                        file => fileId,
                                        file_comment => commentId,
                                        channel => channelId,
                                        timestamp => ts);

            var response = this.request.ExecuteAndDeserializeRequest<ResponseBase>(apiPath);

            return response;
        }
    }
}
