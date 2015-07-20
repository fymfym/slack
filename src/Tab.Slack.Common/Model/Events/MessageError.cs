using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tab.Slack.Common.Model.Events
{
    public class MessageError : EventMessageBase
    {
        public bool Ok { get; set; }
        public long ReplyTo { get; set; }
        public Error Error { get; set; }
    }
}

