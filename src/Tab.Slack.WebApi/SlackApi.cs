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
    public class SlackApi : ISlackApi
    {
        public string ApiKey { get; set; }

        public IRestClient RestClient { get; set; } = new RestClient("https://slack.com/api");
        public IResponseParser ResponseParser { get; set; } = new ResponseParser();

        public SlackApi(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentNullException(nameof(apiKey));

            this.ApiKey = apiKey;
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

        public ChannelsResponse ChannelList(bool excludeArchived = false)
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

        public FileResponse FileUpload(FileUploadRequest request)
        {
            var requestParams = BuildRequestParams(request);
            var response = ExecuteAndDeserializeRequest<FileResponse>("/files.upload", requestParams, file: request);

            return response;
        }

        public ResponseBase GroupArchive(string groupId)
        {
            var apiPath = BuildApiPath("/groups.archive", channel => groupId);
            var response = ExecuteAndDeserializeRequest<ResponseBase>(apiPath);

            return response;
        }

        public CloseResponse GroupClose(string groupId)
        {
            var apiPath = BuildApiPath("/groups.close", channel => groupId);
            var response = ExecuteAndDeserializeRequest<CloseResponse>(apiPath);

            return response;
        }

        public GroupResponse GroupCreate(string groupName)
        {
            var apiPath = BuildApiPath("/groups.create", name => groupName);
            var response = ExecuteAndDeserializeRequest<GroupResponse>(apiPath);

            return response;
        }

        public GroupResponse GroupCreateChild(string groupId)
        {
            var apiPath = BuildApiPath("/groups.createChild", channel => groupId);
            var response = ExecuteAndDeserializeRequest<GroupResponse>(apiPath);

            return response;
        }

        public MessagesResponse GroupHistory(string groupId, string latestTs = null,
            string oldestTs = null, bool isInclusive = false, int messageCount = 100)
        {
            var apiPath = BuildApiPath("/groups.history",
                                        channel => groupId,
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

        public GroupResponse GroupInfo(string groupId)
        {
            var apiPath = BuildApiPath("/groups.info", channel => groupId);
            var response = ExecuteAndDeserializeRequest<GroupResponse>(apiPath);

            return response;
        }

        public GroupResponse GroupInvite(string groupId, string userId)
        {
            var apiPath = BuildApiPath("/groups.invite", channel => groupId, user => userId);
            var response = ExecuteAndDeserializeRequest<GroupResponse>(apiPath);

            return response;
        }

        public ResponseBase GroupKick(string groupId, string userId)
        {
            var apiPath = BuildApiPath("/groups.kick", channel => groupId, user => userId);
            var response = ExecuteAndDeserializeRequest<ResponseBase>(apiPath);

            return response;
        }

        public ResponseBase GroupLeave(string groupId)
        {
            var apiPath = BuildApiPath("/groups.leave", channel => groupId);
            var response = ExecuteAndDeserializeRequest<ResponseBase>(apiPath);

            return response;
        }

        public GroupsResponse GroupList(bool excludeArchived = false)
        {
            var apiPath = BuildApiPath("/groups.list", exclude_archived => excludeArchived ? "1" : "0");
            var response = ExecuteAndDeserializeRequest<GroupsResponse>(apiPath);

            return response;
        }

        public ResponseBase GroupMark(string groupId, string timestamp)
        {
            var apiPath = BuildApiPath("/groups.mark", channel => groupId, ts => timestamp);
            var response = ExecuteAndDeserializeRequest<ResponseBase>(apiPath);

            return response;
        }

        public ResponseBase GroupOpen(string groupId)
        {
            var apiPath = BuildApiPath("/groups.open", channel => groupId);
            var response = ExecuteAndDeserializeRequest<ResponseBase>(apiPath);

            return response;
        }

        public ChannelResponse GroupRename(string groupId, string groupName)
        {
            var apiPath = BuildApiPath("/groups.rename", channel => groupId, name => groupName);
            var response = ExecuteAndDeserializeRequest<ChannelResponse>(apiPath);

            return response;
        }

        public PurposeResponse GroupSetPurpose(string groupId, string groupPurpose)
        {
            var apiPath = BuildApiPath("/groups.setPurpose", channel => groupId, purpose => groupPurpose);
            var response = ExecuteAndDeserializeRequest<PurposeResponse>(apiPath);

            return response;
        }

        public TopicResponse GroupSetTopic(string groupId, string groupTopic)
        {
            var apiPath = BuildApiPath("/groups.setTopic", channel => groupId, topic => groupTopic);
            var response = ExecuteAndDeserializeRequest<TopicResponse>(apiPath);

            return response;
        }

        public ResponseBase GroupUnarchive(string groupId)
        {
            var apiPath = BuildApiPath("/groups.unarchive", channel => groupId);
            var response = ExecuteAndDeserializeRequest<ResponseBase>(apiPath);

            return response;
        }

        public CloseResponse ImClose(string imId)
        {
            var apiPath = BuildApiPath("/im.close", channel => imId);
            var response = ExecuteAndDeserializeRequest<CloseResponse>(apiPath);

            return response;
        }

        public MessagesResponse ImHistory(string imId, string latestTs = null,
            string oldestTs = null, bool isInclusive = false, int messageCount = 100)
        {
            var apiPath = BuildApiPath("/im.history",
                                        channel => imId,
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

        public ImsResponse ImList(bool excludeArchived = false)
        {
            var apiPath = BuildApiPath("/im.list", exclude_archived => excludeArchived ? "1" : "0");
            var response = ExecuteAndDeserializeRequest<ImsResponse>(apiPath);

            return response;
        }

        public ResponseBase ImMark(string imId, string timestamp)
        {
            var apiPath = BuildApiPath("/im.mark", channel => imId, ts => timestamp);
            var response = ExecuteAndDeserializeRequest<ResponseBase>(apiPath);

            return response;
        }

        public ImOpenResponse ImOpen(string userId)
        {
            var apiPath = BuildApiPath("/im.open", user => userId);
            var response = ExecuteAndDeserializeRequest<ImOpenResponse>(apiPath);

            return response;
        }

        public OauthAccessResponse OauthAccess(string clientId, string clientSecret, string callbackCode, string redirectUri = null)
        {
            var apiPath = BuildApiPath("/oauth.access", client_id => clientId, client_secret => clientSecret, code => callbackCode, redirect_uri => redirectUri);
            var response = ExecuteAndDeserializeRequest<OauthAccessResponse>(apiPath);

            return response;
        }

        public ResponseBase ReactionAdd(string reaction, string fileId = null, string commentId = null, string channelId = null, string ts = null)
        {
            var apiPath = BuildApiPath("/reactions.add",
                                        name => reaction,
                                        file => fileId,
                                        file_comment => commentId,
                                        channel => channelId,
                                        timestamp => ts);

            var response = ExecuteAndDeserializeRequest<ResponseBase>(apiPath);

            return response;
        }

        public ReactionItem ReactionGet(string fileId = null, string commentId = null, string channelId = null, string ts = null, bool? fullResults = null)
        {
            var apiPath = BuildApiPath("/reactions.get",
                                        file => fileId,
                                        file_comment => commentId,
                                        channel => channelId,
                                        timestamp => ts,
                                        full => fullResults);

            var response = ExecuteAndDeserializeRequest<ReactionItem>(apiPath);

            return response;
        }

        public ReactionListResponse ReactionList(string userId = null, bool? fullResults = null, int? reactionCount = null, int? pageNumber = null)
        {
            var apiPath = BuildApiPath("/reactions.list",
                                        user => userId,
                                        full => fullResults,
                                        page => pageNumber,
                                        count => reactionCount);

            var response = ExecuteAndDeserializeRequest<ReactionListResponse>(apiPath);

            return response;
        }

        public ResponseBase ReactionRemove(string reaction, string fileId = null, string commentId = null, string channelId = null, string ts = null)
        {
            var apiPath = BuildApiPath("/reactions.remove",
                                        name => reaction,
                                        file => fileId,
                                        file_comment => commentId,
                                        channel => channelId,
                                        timestamp => ts);

            var response = ExecuteAndDeserializeRequest<ResponseBase>(apiPath);

            return response;
        }

        public SearchResponse SearchAll(string queryString, SearchSortType? sortType = null,
            SortDirection? sortDir = null, bool? isHighlighted = null, int? messageCount = null, 
            int? pageNumber = null)
        {
            string highlighted = null;

            if (isHighlighted.HasValue)
                highlighted = isHighlighted.Value ? "1" : "0";

            var apiPath = BuildApiPath("/search.all",
                                        query => queryString,
                                        sort => sortType,
                                        sort_dir => sortDir,
                                        highlight => highlighted,
                                        count => messageCount,
                                        page => pageNumber);

            var response = ExecuteAndDeserializeRequest<SearchResponse>(apiPath);

            return response;
        }

        public SearchResponse SearchFiles(string queryString, SearchSortType? sortType = null,
            SortDirection? sortDir = null, bool? isHighlighted = null, int? messageCount = null,
            int? pageNumber = null)
        {
            string highlighted = null;

            if (isHighlighted.HasValue)
                highlighted = isHighlighted.Value ? "1" : "0";

            var apiPath = BuildApiPath("/search.files",
                                        query => queryString,
                                        sort => sortType,
                                        sort_dir => sortDir,
                                        highlight => highlighted,
                                        count => messageCount,
                                        page => pageNumber);

            var response = ExecuteAndDeserializeRequest<SearchResponse>(apiPath);

            return response;
        }

        public SearchResponse SearchMessages(string queryString, SearchSortType? sortType = null,
            SortDirection? sortDir = null, bool? isHighlighted = null, int? messageCount = null,
            int? pageNumber = null)
        {
            string highlighted = null;

            if (isHighlighted.HasValue)
                highlighted = isHighlighted.Value ? "1" : "0";

            var apiPath = BuildApiPath("/search.messages",
                                        query => queryString,
                                        sort => sortType,
                                        sort_dir => sortDir,
                                        highlight => highlighted,
                                        count => messageCount,
                                        page => pageNumber);

            var response = ExecuteAndDeserializeRequest<SearchResponse>(apiPath);

            return response;
        }

        public StarsResponse StarsList(string userId = null, int? messageCount = null, int? pageNumber = null)
        {
            var apiPath = BuildApiPath("/stars.list",
                                        user => userId,
                                        count => messageCount,
                                        page => pageNumber);

            var response = ExecuteAndDeserializeRequest<StarsResponse>(apiPath);

            return response;
        }

        public TeamAccessLogs TeamAccessLogs(int? messageCount = null, int? pageNumber = null)
        {
            var apiPath = BuildApiPath("/team.accessLogs", count => messageCount, page => pageNumber);
            var response = ExecuteAndDeserializeRequest<TeamAccessLogs>(apiPath);

            return response;
        }

        public TeamResponse TeamInfo()
        {
            var response = ExecuteAndDeserializeRequest<TeamResponse>("/team.info");

            return response;
        }

        public PresenceResponse UsersGetPresence(string userId)
        {
            var apiPath = BuildApiPath("/users.getPresence", user => userId);
            var response = ExecuteAndDeserializeRequest<PresenceResponse>(apiPath);

            return response;
        }

        public UserResponse UsersInfo(string userId)
        {
            var apiPath = BuildApiPath("/users.info", user => userId);
            var response = ExecuteAndDeserializeRequest<UserResponse>(apiPath);

            return response;
        }

        public UsersResponse UsersList()
        {
            var response = ExecuteAndDeserializeRequest<UsersResponse>("/users.list");

            return response;
        }

        public ResponseBase UsersSetActive()
        {
            var response = ExecuteAndDeserializeRequest<ResponseBase>("/users.setActive");

            return response;
        }

        public ResponseBase UsersSetPresence(string presenceValue = "auto")
        {
            var apiPath = BuildApiPath("/users.setPresence", presence => presenceValue);
            var response = ExecuteAndDeserializeRequest<ResponseBase>(apiPath);

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
                // TODO: maybe easier to whitelist types instead of blacklist
                if (paramProp.PropertyType.IsAssignableFrom(typeof(byte[])))
                    continue;

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

        private string BuildApiPath(string apiPath, params Expression<Func<string, object>>[] queryParamParts)
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

                queryParams.Add($"{key}={Uri.EscapeDataString(value.ToString())}");
            }

            return $"{apiPath}?" + string.Join("&", queryParams);
        }

        private T ExecuteAndDeserializeRequest<T>(string apiPath, Dictionary<string, string> parameters = null, Method method = Method.POST, FileUploadRequest file = null)
        {
            var response = ExecuteRequest(apiPath, parameters, method, file);

            // TODO: handle error response

            var result = this.ResponseParser.Deserialize<T>(response.Content);

            return result;
        }

        private IRestResponse ExecuteRequest(string apiPath, Dictionary<string, string> parameters = null, Method method = Method.POST, FileUploadRequest file = null)
        {
            var request = new RestRequest(apiPath, method);
            request.AddParameter("token", this.ApiKey);

            if (file != null)
                request.AddFile("file", file.FileData, file.Filename);
            
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
