using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model;
using Tab.Slack.Common.Model.Events;
using Tab.Slack.Common.Model.Requests;
using Tab.Slack.Common.Model.Responses;

namespace Tab.Slack.WebApi
{
    public class RtmApi : IRtmApi
    {
        private readonly IRequestHandler request;

        public RtmApi(IRequestHandler requestHandler)
        {
            if (requestHandler == null)
                throw new ArgumentNullException(nameof(requestHandler));

            this.request = requestHandler;
        }

        public RtmStartResponse Start()
        {
            var response = this.request.ExecuteAndDeserializeRequest<RtmStartResponse>("/rtm.start");

            if (response != null)
                response.Type = EventType.RtmStart;

            return response;
        }
    }
}
