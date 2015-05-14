using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tab.Slack.Common.Model.Events
{
    public class StarAdded : EventMessageBase
    {
        public string User { get; set; }
        public Item Item { get; set; }
        public string EventTs { get; set; }
    }
}

