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
    public class OauthApi : IOauthApi
    {
        private readonly IRequestHandler request;

        public OauthApi(IRequestHandler requestHandler)
        {
            if (requestHandler == null)
                throw new ArgumentNullException(nameof(requestHandler));

            this.request = requestHandler;
        }

        public OauthAccessResponse Access(string clientId, string clientSecret, string callbackCode, string redirectUri = null)
        {
            var apiPath = this.request.BuildApiPath("/oauth.access", client_id => clientId, client_secret => clientSecret, code => callbackCode, redirect_uri => redirectUri);
            var response = this.request.ExecuteAndDeserializeRequest<OauthAccessResponse>(apiPath);

            return response;
        }
    }
}
