using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tab.Slack.Common.Model.Events
{
    public class GroupOpen : EventMessageBase
    {
        public string User { get; set; }
        public string Channel { get; set; }
    }
}

