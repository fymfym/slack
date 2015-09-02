using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tab.Slack.Common.Model.Events
{
    public class PinAdded : EventMessageBase
    {
        public string User { get; set; }
        public string ChannelId { get; set; }
        public string EventTs { get; set; }

        // TODO ...
    }
}

