using Tab.Slack.Common.Json;
using Tab.Slack.Common.Model;
using Tab.Slack.Common.Model.Requests;
using Tab.Slack.Common.Model.Responses;

namespace Tab.Slack.WebApi
{
    public interface ISlackApi
    {
        IResponseParser ResponseParser { get; set; }
        string ApiKey { get; set; }

        RtmStartResponse RtmStart();
        ApiTestResponse ApiTest(string error = null, params string[] args);
        AuthTestResponse AuthTest();
        ResponseBase ChannelArchive(string channelId);
        ChannelResponse ChannelCreate(string channelName);
        MessagesResponse ChannelHistory(string channelId, string latestTs = null,
            string oldestTs = null, bool isInclusive = false, int messageCount = 100);
        ChannelResponse ChannelInfo(string channelId);
        ChannelResponse ChannelInvite(string channelId, string userId);
        ChannelResponse ChannelJoin(string channelName);
        ResponseBase ChannelKick(string channelId, string userId);
        LeaveChannelResponse ChannelLeave(string channelId);
        ChannelsResponse ChannelList(bool excludeArchived = false);
        ResponseBase ChannelMark(string channelId, string timestamp);
        ChannelResponse ChannelRename(string channelId, string channelName);
        PurposeResponse ChannelSetPurpose(string channelId, string channelPurpose);
        TopicResponse ChannelSetTopic(string channelId, string channelTopic);
        ResponseBase ChannelUnarchive(string channelId);
        ChatDeleteResponse ChatDelete(string channelId, string timestamp);
        MessageResponse ChatPostMessage(PostMessageRequest request);
        ChatUpdateResponse ChatUpdate(string channelId, string timestamp, string updatedText);
        EmojiResponse EmojiList();
        ResponseBase FileDelete(string fileId);
        FileResponse FileInfo(string fileId, int commentsCount = 100, int pageNumber = 1);
        FilesResponse FileList(FilesListRequest request);
        FileResponse FileUpload(FileUploadRequest request);
        ResponseBase GroupArchive(string groupId);
        CloseResponse GroupClose(string groupId);
        GroupResponse GroupCreate(string groupName);
        GroupResponse GroupCreateChild(string groupId);
        MessagesResponse GroupHistory(string groupId, string latestTs = null,
            string oldestTs = null, bool isInclusive = false, int messageCount = 100);
        GroupResponse GroupInfo(string groupId);
        GroupResponse GroupInvite(string groupId, string userId);
        ResponseBase GroupKick(string groupId, string userId);
        ResponseBase GroupLeave(string groupId);
        GroupsResponse GroupList(bool excludeArchived = false);
        ResponseBase GroupMark(string groupId, string timestamp);
        ResponseBase GroupOpen(string groupId);
        ChannelResponse GroupRename(string groupId, string groupName);
        PurposeResponse GroupSetPurpose(string groupId, string groupPurpose);
        TopicResponse GroupSetTopic(string groupId, string groupTopic);
        ResponseBase GroupUnarchive(string groupId);
        CloseResponse ImClose(string imId);
        MessagesResponse ImHistory(string imId, string latestTs = null,
            string oldestTs = null, bool isInclusive = false, int messageCount = 100);
        ImsResponse ImList(bool excludeArchived = false);
        ResponseBase ImMark(string imId, string timestamp);
        ImOpenResponse ImOpen(string userId);
        OauthAccessResponse OauthAccess(string clientId, string clientSecret, string callbackCode, string redirectUri = null);
        ResponseBase ReactionAdd(string reaction, string fileId = null, string commentId = null, string channelId = null, string ts = null);
        ReactionItem ReactionGet(string fileId = null, string commentId = null, string channelId = null, string ts = null, bool? fullResults = null);
        ReactionListResponse ReactionList(string userId = null, bool? fullResults = null, int? reactionCount = null, int? pageNumber = null);
        ResponseBase ReactionRemove(string reaction, string fileId = null, string commentId = null, string channelId = null, string ts = null);
        SearchResponse SearchAll(string queryString, SearchSortType? sortType = null,
            SortDirection? sortDir = null, bool? isHighlighted = null, int? messageCount = null,
            int? pageNumber = null);
        SearchResponse SearchFiles(string queryString, SearchSortType? sortType = null,
            SortDirection? sortDir = null, bool? isHighlighted = null, int? messageCount = null,
            int? pageNumber = null);
        SearchResponse SearchMessages(string queryString, SearchSortType? sortType = null,
            SortDirection? sortDir = null, bool? isHighlighted = null, int? messageCount = null,
            int? pageNumber = null);

        StarsResponse StarsList(string userId = null, int? messageCount = null, int? pageNumber = null);
        TeamAccessLogs TeamAccessLogs(int? messageCount = null, int? pageNumber = null);
        TeamResponse TeamInfo();
        PresenceResponse UsersGetPresence(string userId);
        UserResponse UsersInfo(string userId);
        UsersResponse UsersList();
        ResponseBase UsersSetActive();
        ResponseBase UsersSetPresence(string presenceValue = "auto");
    }
}