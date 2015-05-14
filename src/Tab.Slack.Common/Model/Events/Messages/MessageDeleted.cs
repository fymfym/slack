using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tab.Slack.Common.Model.Events.Messages
{
    public class MessageDeleted : MessageBase
    {
        public bool Hidden { get; set; }
        public string DeletedTs { get; set; }
    }
}

