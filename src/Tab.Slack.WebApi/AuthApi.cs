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
    public class AuthApi : IAuthApi
    {
        private readonly IRequestHandler request;

        public AuthApi(IRequestHandler requestHandler)
        {
            if (requestHandler == null)
                throw new ArgumentNullException(nameof(requestHandler));

            this.request = requestHandler;
        }

        public AuthTestResponse Test()
        {
            var response = this.request.ExecuteAndDeserializeRequest<AuthTestResponse>("/auth.test");

            return response;
        }
    }
}
