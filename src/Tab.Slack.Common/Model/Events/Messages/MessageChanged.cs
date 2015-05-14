using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tab.Slack.Common.Model.Events.Messages
{
    public class MessageChanged : MessageBase
    {
        public bool Hidden { get; set; }
        public string EventTs { get; set; }
        public EditedMessage Message { get; set; }   
    }
}

