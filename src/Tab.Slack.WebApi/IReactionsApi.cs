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
    public interface IReactionsApi
    {
        ResponseBase Add(string reaction, string fileId = null, string commentId = null, string channelId = null, string ts = null);
        ReactionItem Get(string fileId = null, string commentId = null, string channelId = null, string ts = null, bool? fullResults = null);
        ReactionListResponse List(string userId = null, bool? fullResults = null, int? reactionCount = null, int? pageNumber = null);
        ResponseBase Remove(string reaction, string fileId = null, string commentId = null, string channelId = null, string ts = null);
    }
}
