using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model.Requests;
using Tab.Slack.Common.Model.Responses;

namespace Tab.Slack.WebApi
{
    public interface IImApi
    {
        CloseResponse Close(string imId);
        MessagesResponse History(string imId, string latestTs = null,
            string oldestTs = null, bool isInclusive = false, int messageCount = 100);
        ImsResponse List(bool excludeArchived = false);
        ResponseBase Mark(string imId, string timestamp);
        ImOpenResponse Open(string userId);
    }
}
