using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tab.Slack.Common.Model.Events.Messages
{
    public class BotMessage : MessageBase
    {
        public string BotId { get; set; }
        public string Username { get; set; }
        public BotIcons Icons { get; set; }
    }
}

