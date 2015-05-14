using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            var response = ExecuteAndDeserializeRequest<RtmStartResponse>("/rtm.start");

            if (response != null)
                response.Type = EventType.RtmStart;

            return response;
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

            var response = ExecuteAndDeserializeRequest<ApiTestResponse>(request);

            return response;
        }

        public AuthTestResponse AuthTest()
        {
            var response = ExecuteAndDeserializeRequest<AuthTestResponse>("/auth.test");
            
            return response;
        }

        public ResponseBase ChannelArchive(string channelId)
        {
            var apiPath = BuildApiPath("/channels.archive", channel => channelId);
            var response = ExecuteAndDeserializeRequest<ResponseBase>(apiPath);

            return response;
        }

        public ChannelResponse ChannelCreate(string channelName)
        {
            var apiPath = BuildApiPath("/channels.create", name => channelName);
            var response = ExecuteAndDeserializeRequest<ChannelResponse>(apiPath);

            return response;
        }

        public MessagesResponse ChannelHistory(string channelId, string latestTs = null,
            string oldestTs = null, bool isInclusive = false, int messageCount = 100)
        {
            var apiPath = BuildApiPath("/channels.history", 
                                        channel => channelId, 
                                        latest => latestTs, 
                                        oldest => oldestTs, 
                                        inclusive => isInclusive ? "1" : "0", 
                                        count => messageCount.ToString());

            var response = ExecuteAndDeserializeRequest<MessagesResponse>(apiPath);

            if (response.Messages != null)
            {
                response.Messages = this.ResponseParser.RemapMessagesToConcreteTypes(response.Messages)
                                                       .ToList();
            }

            return response;
        }

        public ChannelResponse ChannelInfo(string channelId)
        {
            var apiPath = BuildApiPath("/channels.info", channel => channelId);
            var response = ExecuteAndDeserializeRequest<ChannelResponse>(apiPath);

            return response;
        }

        public ChannelResponse ChannelInvite(string channelId, string userId)
        {
            var apiPath = BuildApiPath("/channels.invite", channel => channelId, user => userId);
            var response = ExecuteAndDeserializeRequest<ChannelResponse>(apiPath);

            return response;
        }

        public ChannelResponse ChannelJoin(string channelName)
        {
            var apiPath = BuildApiPath("/channels.join", name => channelName);
            var response = ExecuteAndDeserializeRequest<ChannelResponse>(apiPath);

            return response;
        }

        private string BuildApiPath(string apiPath, params Expression<Func<string, string>>[] queryParamParts)
        {
            if (queryParamParts == null)
                return apiPath;

            var queryParams = new List<string>();

            foreach (var paramPart in queryParamParts)
            {
                var key = paramPart.Parameters[0].Name;
                var value = paramPart.Compile().Invoke("");

                if (value == null)
                    continue;

                queryParams.Add($"{key}={Uri.EscapeDataString(value)}");
            }

            return $"{apiPath}?" + string.Join("&", queryParams);
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
