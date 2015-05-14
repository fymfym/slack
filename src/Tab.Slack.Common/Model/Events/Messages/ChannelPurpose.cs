using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tab.Slack.Common.Model.Events.Messages
{
    public class ChannelPurpose : MessageBase
    {
        public string Purpose { get; set; }
    }
}

