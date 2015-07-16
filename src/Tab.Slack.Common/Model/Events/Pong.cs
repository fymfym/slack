using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tab.Slack.Common.Model.Events
{
    public class Pong : EventMessageBase
    {
        public string ReplyTo { get; set; }
        public string Time { get; set; }
    }
}
