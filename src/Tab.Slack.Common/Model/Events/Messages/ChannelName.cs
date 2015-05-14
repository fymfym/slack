using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tab.Slack.Common.Model.Events.Messages
{
    public class ChannelName : MessageBase
    {
        public string OldName { get; set; }
        public string Name { get; set; }
    }
}

