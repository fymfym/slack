using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model.Requests;
using Tab.Slack.Common.Model.Responses;

namespace Tab.Slack.WebApi
{
    public class TeamApi : ITeamApi
    {
        private readonly IRequestHandler request;

        public TeamApi(IRequestHandler requestHandler)
        {
            if (requestHandler == null)
                throw new ArgumentNullException(nameof(requestHandler));

            this.request = requestHandler;
        }

        public TeamAccessLogs AccessLogs(int? messageCount = null, int? pageNumber = null)
        {
            var apiPath = this.request.BuildApiPath("/team.accessLogs", count => messageCount, page => pageNumber);
            var response = this.request.ExecuteAndDeserializeRequest<TeamAccessLogs>(apiPath);

            return response;
        }

        public TeamResponse Info()
        {
            var response = this.request.ExecuteAndDeserializeRequest<TeamResponse>("/team.info");

            return response;
        }
    }
}
