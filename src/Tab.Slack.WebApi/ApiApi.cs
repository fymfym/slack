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
    public class ApiApi : IApiApi
    {
        private readonly IRequestHandler request;

        public ApiApi(IRequestHandler requestHandler)
        {
            if (requestHandler == null)
                throw new ArgumentNullException(nameof(requestHandler));

            this.request = requestHandler;
        }

        public ApiTestResponse Test(string error = null, params string[] args)
        {
            var queryParams = new List<string>();

            if (error != null)
                queryParams.Add($"error={Uri.EscapeDataString(error)}");

            int argIndex = 1;

            foreach (var arg in args ?? Enumerable.Empty<string>())
            {
                queryParams.Add($"arg{argIndex++}={Uri.EscapeDataString(arg)}");
            }

            var request = "/api.test?" + string.Join("&", queryParams);

            var response = this.request.ExecuteAndDeserializeRequest<ApiTestResponse>(request);

            return response;
        }
    }
}
