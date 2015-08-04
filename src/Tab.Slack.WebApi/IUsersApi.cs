using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model.Requests;
using Tab.Slack.Common.Model.Responses;

namespace Tab.Slack.WebApi
{
    public interface IUsersApi
    {
        PresenceResponse GetPresence(string userId);
        UserResponse Info(string userId);
        UsersResponse List();
        ResponseBase SetActive();
        ResponseBase SetPresence(string presenceValue = "auto");
    }
}
