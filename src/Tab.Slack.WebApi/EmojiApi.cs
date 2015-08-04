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
    public class EmojiApi : IEmojiApi
    {
        private readonly IRequestHandler request;

        public EmojiApi(IRequestHandler requestHandler)
        {
            if (requestHandler == null)
                throw new ArgumentNullException(nameof(requestHandler));

            this.request = requestHandler;
        }

        public EmojiResponse List()
        {
            var response = this.request.ExecuteAndDeserializeRequest<EmojiResponse>("/emoji.list");

            return response;
        }
    }
}
