using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tab.Slack.Common.Model.Responses
{
    public class ChatDeleteResponse : ResponseBase
    {
        public string Channel { get; set; }
        public string Ts { get; set; }
    }
}
