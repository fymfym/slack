using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model.Events.Messages;

namespace Tab.Slack.Common.Model.Responses
{
    public class MessageResponse : ResponseBase
    {
        public string Ts { get; set; }
        public string Channel { get; set; }
        public MessageBase Message { get; set; }
    }
}
