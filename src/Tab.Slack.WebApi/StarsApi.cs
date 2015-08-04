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
    public class StarsApi : IStarsApi
    {
        private readonly IRequestHandler request;

        public StarsApi(IRequestHandler requestHandler)
        {
            if (requestHandler == null)
                throw new ArgumentNullException(nameof(requestHandler));

            this.request = requestHandler;
        }

        public StarsResponse List(string userId = null, int? messageCount = null, int? pageNumber = null)
        {
            var apiPath = this.request.BuildApiPath("/stars.list",
                                        user => userId,
                                        count => messageCount,
                                        page => pageNumber);

            var response = this.request.ExecuteAndDeserializeRequest<StarsResponse>(apiPath);

            return response;
        }
    }
}
