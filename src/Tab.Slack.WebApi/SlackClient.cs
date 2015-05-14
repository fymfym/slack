using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Json;
using Tab.Slack.Common.Model;
using Tab.Slack.Common.Model.Events;

namespace Tab.Slack.WebApi
{
    public class SlackClient : ISlackClient
    {
        private readonly RestClient restClient;
        private readonly string apiKey;

        public IResponseParser ResponseParser { get; set; } = new ResponseParser();

        public SlackClient(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentNullException(nameof(apiKey));

            this.apiKey = apiKey;
            this.restClient = new RestClient("https://slack.com/api");
        }

        public RtmStartResponse RtmStart()
        {
            var response = ExecuteRequest("/rtm.start");
            var rtmStartResponse = this.ResponseParser.DeserializeEvent<RtmStartResponse>(response.Content);

            if (rtmStartResponse != null)
                rtmStartResponse.Type = EventType.RtmStart;

            return rtmStartResponse;
        }

        private IRestResponse ExecuteRequest(string apiPath, Method method = Method.POST)
        {
            var request = new RestRequest(apiPath, method);
            request.AddParameter("token", this.apiKey);

            return this.restClient.Execute(request);
        }
    }
}
