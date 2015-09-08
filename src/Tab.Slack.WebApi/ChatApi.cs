using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model.Requests;
using Tab.Slack.Common.Model.Responses;

namespace Tab.Slack.WebApi
{
    public class ChatApi : IChatApi
    {
        private readonly IRequestHandler request;

        public ChatApi(IRequestHandler requestHandler)
        {
            if (requestHandler == null)
                throw new ArgumentNullException(nameof(requestHandler));

            this.request = requestHandler;
        }

        public ChatDeleteResponse Delete(string channelId, string timestamp)
        {
            var apiPath = this.request.BuildApiPath("/chat.delete", channel => channelId, ts => timestamp);
            var response = this.request.ExecuteAndDeserializeRequest<ChatDeleteResponse>(apiPath);

            return response;
        }

        public MessageResponse PostMessage(PostMessageRequest request)
        {
            var requestParams = this.request.BuildRequestParams(request);
            var response = this.request.ExecuteAndDeserializeRequest<MessageResponse>("/chat.postMessage", requestParams);

            if (response.Ok)
                response.Message = this.request.ResponseParser.RemapMessageToConcreteType(response.Message);

            return response;
        }

        public ChatUpdateResponse Update(string channelId, string timestamp, string updatedText)
        {
            var apiPath = this.request.BuildApiPath("/chat.update", channel => channelId, ts => timestamp, text => updatedText);
            var response = this.request.ExecuteAndDeserializeRequest<ChatUpdateResponse>(apiPath);

            return response;
        }
    }
}
