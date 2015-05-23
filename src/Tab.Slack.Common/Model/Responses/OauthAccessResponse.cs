using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tab.Slack.Common.Model.Responses
{
    public class OauthAccessResponse : ResponseBase
    {
        public string AccessToken { get; set; }
        public string Scope { get; set; }
    }
}
