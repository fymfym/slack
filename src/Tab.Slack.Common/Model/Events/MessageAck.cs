using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tab.Slack.Common.Model.Events
{
    public class MessageAck : EventMessageBase
    {
        public bool Ok { get; set; }
        public long ReplyTo { get; set; }
        public string Ts { get; set; }
        public string Text { get; set; }
    }
}

