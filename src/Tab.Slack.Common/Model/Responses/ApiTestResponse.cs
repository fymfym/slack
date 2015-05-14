using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tab.Slack.Common.Model.Responses
{
    public class ApiTestResponse : ResponseBase
    {
        public string Error { get; set; }
        public IDictionary<string, string> Args { get; set; }
    }
}
