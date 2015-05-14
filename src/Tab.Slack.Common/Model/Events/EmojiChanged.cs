using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tab.Slack.Common.Model.Events
{
    public class EmojiChanged : EventMessageBase
    {
        public string EventTs { get; set; }
    }
}

