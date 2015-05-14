using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model.Events.Messages;

namespace Tab.Slack.Common.Model.Responses
{
    public class MessagesResponse : ResponseBase
    {
        public string Latest { get; set; }
        public List<MessageBase> Messages { get; set; }
        public bool HasMore { get; set; }
    }
}
