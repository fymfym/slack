using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Json;
using Tab.Slack.Common.Model;
using Tab.Slack.Common.Model.Events;
using Tab.Slack.Common.Model.Responses;

namespace Tab.Slack.WebApi
{
    public class SlackClient : ISlackClient
    {
        private readonly string apiKey;

        public IRestClient RestClient { get; set; } = new RestClient("https://slack.com/api");
        public IResponseParser ResponseParser { get; set; } = new ResponseParser();

        public SlackClient(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentNullException(nameof(apiKey));

            this.apiKey = apiKey;
        }

        public RtmStartResponse RtmStart()
        {
            var rtmStartResponse = ExecuteAndDeserializeRequest<RtmStartResponse>("/rtm.start");

            if (rtmStartResponse != null)
                rtmStartResponse.Type = EventType.RtmStart;

            return rtmStartResponse;
        }

        public ApiTestResponse ApiTest(string error = null, params string[] args)
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

            var testResponse = ExecuteAndDeserializeRequest<ApiTestResponse>(request);

            return testResponse;
        }

        public AuthTestResponse AuthTest()
        {
            var testResponse = ExecuteAndDeserializeRequest<AuthTestResponse>("/auth.test");

            return testResponse;
        }

        private T ExecuteAndDeserializeRequest<T>(string apiPath, Method method = Method.POST)
        {
            var response = ExecuteRequest(apiPath, method);
            var result = this.ResponseParser.Deserialize<T>(response.Content);

            return result;
        }

        private IRestResponse ExecuteRequest(string apiPath, Method method = Method.POST)
        {
            var request = new RestRequest(apiPath, method);
            request.AddParameter("token", this.apiKey);

            return this.RestClient.Execute(request);
        }
    }
}
