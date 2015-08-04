using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model.Requests;
using Tab.Slack.Common.Model.Responses;

namespace Tab.Slack.WebApi
{
    public class UsersApi : IUsersApi
    {
        private readonly IRequestHandler request;

        public UsersApi(IRequestHandler requestHandler)
        {
            if (requestHandler == null)
                throw new ArgumentNullException(nameof(requestHandler));

            this.request = requestHandler;
        }

        public PresenceResponse GetPresence(string userId)
        {
            var apiPath = this.request.BuildApiPath("/users.getPresence", user => userId);
            var response = this.request.ExecuteAndDeserializeRequest<PresenceResponse>(apiPath);

            return response;
        }

        public UserResponse Info(string userId)
        {
            var apiPath = this.request.BuildApiPath("/users.info", user => userId);
            var response = this.request.ExecuteAndDeserializeRequest<UserResponse>(apiPath);

            return response;
        }

        public UsersResponse List()
        {
            var response = this.request.ExecuteAndDeserializeRequest<UsersResponse>("/users.list");

            return response;
        }

        public ResponseBase SetActive()
        {
            var response = this.request.ExecuteAndDeserializeRequest<ResponseBase>("/users.setActive");

            return response;
        }

        public ResponseBase SetPresence(string presenceValue = "auto")
        {
            var apiPath = this.request.BuildApiPath("/users.setPresence", presence => presenceValue);
            var response = this.request.ExecuteAndDeserializeRequest<ResponseBase>(apiPath);

            return response;
        }
    }
}
