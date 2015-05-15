using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Json;
using Tab.Slack.Common.Model;
using Tab.Slack.Common.Model.Events;
using Tab.Slack.Common.Model.Requests;
using Tab.Slack.Common.Model.Responses;

namespace Tab.Slack.WebApi
{
    // TODO: read up on this strange "Single Responsibility Principle" people talk of
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

        public ResponseBase ChannelKick(string channelId, string userId)
        {
            var apiPath = BuildApiPath("/channels.kick", channel => channelId, user => userId);
            var response = ExecuteAndDeserializeRequest<ResponseBase>(apiPath);

            return response;
        }

        public LeaveChannelResponse ChannelLeave(string channelId)
        {
            var apiPath = BuildApiPath("/channels.leave", channel => channelId);
            var response = ExecuteAndDeserializeRequest<LeaveChannelResponse>(apiPath);

            return response;
        }

        public ChannelsResponse ChannelsList(bool excludeArchived = false)
        {
            var apiPath = BuildApiPath("/channels.list", exclude_archived => excludeArchived ? "1" : "0");
            var response = ExecuteAndDeserializeRequest<ChannelsResponse>(apiPath);

            return response;
        }

        public ResponseBase ChannelMark(string channelId, string timestamp)
        {
            var apiPath = BuildApiPath("/channels.mark", channel => channelId, ts => timestamp);
            var response = ExecuteAndDeserializeRequest<ResponseBase>(apiPath);

            return response;
        }

        public ChannelResponse ChannelRename(string channelId, string channelName)
        {
            var apiPath = BuildApiPath("/channels.rename", channel => channelId, name => channelName);
            var response = ExecuteAndDeserializeRequest<ChannelResponse>(apiPath);

            return response;
        }

        public PurposeResponse ChannelSetPurpose(string channelId, string channelPurpose)
        {
            var apiPath = BuildApiPath("/channels.setPurpose", channel => channelId, purpose => channelPurpose);
            var response = ExecuteAndDeserializeRequest<PurposeResponse>(apiPath);

            return response;
        }

        public TopicResponse ChannelSetTopic(string channelId, string channelTopic)
        {
            var apiPath = BuildApiPath("/channels.setTopic", channel => channelId, topic => channelTopic);
            var response = ExecuteAndDeserializeRequest<TopicResponse>(apiPath);

            return response;
        }

        public ResponseBase ChannelUnarchive(string channelId)
        {
            var apiPath = BuildApiPath("/channels.unarchive", channel => channelId);
            var response = ExecuteAndDeserializeRequest<ResponseBase>(apiPath);

            return response;
        }

        public ChatDeleteResponse ChatDelete(string channelId, string timestamp)
        {
            var apiPath = BuildApiPath("/chat.delete", channel => channelId, ts => timestamp);
            var response = ExecuteAndDeserializeRequest<ChatDeleteResponse>(apiPath);

            return response;
        }

        public MessageResponse ChatPostMessage(PostMessageRequest request)
        {
            var requestParams = BuildRequestParams(request);
            var response = ExecuteAndDeserializeRequest<MessageResponse>("/chat.postMessage", requestParams);
            response.Message = this.ResponseParser.RemapMessageToConcreteType(response.Message);

            return response;
        }

        public ChatUpdateResponse ChatUpdate(string channelId, string timestamp, string updatedText)
        {
            var apiPath = BuildApiPath("/chat.update", channel => channelId, ts => timestamp, text => updatedText);
            var response = ExecuteAndDeserializeRequest<ChatUpdateResponse>(apiPath);

            return response;
        }

        public EmojiResponse EmojiList()
        {
            var response = ExecuteAndDeserializeRequest<EmojiResponse>("/emoji.list");

            return response;
        }

        public ResponseBase FileDelete(string fileId)
        {
            var apiPath = BuildApiPath("/files.delete", file => fileId);
            var response = ExecuteAndDeserializeRequest<ChatUpdateResponse>(apiPath);

            return response;
        }

        public FileResponse FileInfo(string fileId, int commentsCount = 100, int pageNumber = 1)
        {
            var apiPath = BuildApiPath("/files.info", 
                                       file => fileId, 
                                       count => commentsCount.ToString(), 
                                       page => pageNumber.ToString()
                                      );
            var response = ExecuteAndDeserializeRequest<FileResponse>(apiPath);

            return response;
        }

        public FilesResponse FileList(FilesListRequest request)
        {
            var requestParams = BuildRequestParams(request);
            var response = ExecuteAndDeserializeRequest<FilesResponse>("/files.list", requestParams);

            return response;
        }

        private Dictionary<string, string> BuildRequestParams<T>(T requestParamsObject)
        {
            if (requestParamsObject == null)
                return new Dictionary<string, string>();

            var requestParams = new Dictionary<string, string>();
            var publicProps = typeof(T).GetProperties();

            foreach (var paramProp in publicProps)
            {
                var key = paramProp.Name.ToDelimitedString('_');
                object value = paramProp.GetMethod.Invoke(requestParamsObject, null);

                if (value == null)
                    continue;

                var stringValue = value.ToString();

                if (value is bool)
                    stringValue = stringValue.ToLower();
                else if (value.GetType().IsEnum)
                    stringValue = stringValue.ToDelimitedString('_');
                else if (!value.GetType().IsPrimitive && !(value is string))
                    stringValue = this.ResponseParser.SerializeMessage(value);

                requestParams.Add(key, stringValue);
            }

            return requestParams;
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

        private T ExecuteAndDeserializeRequest<T>(string apiPath, Dictionary<string, string> parameters = null, Method method = Method.POST)
        {
            var response = ExecuteRequest(apiPath, parameters, method);
            var result = this.ResponseParser.Deserialize<T>(response.Content);

            return result;
        }

        private IRestResponse ExecuteRequest(string apiPath, Dictionary<string, string> parameters = null, Method method = Method.POST)
        {
            var request = new RestRequest(apiPath, method);
            request.AddParameter("token", this.apiKey);

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    if (parameter.Value == null)
                        continue;

                    request.AddParameter(parameter.Key, parameter.Value);
                }
            }

            return this.RestClient.Execute(request);
        }
    }
}
