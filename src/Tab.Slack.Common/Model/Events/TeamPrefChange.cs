using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tab.Slack.Common.Model.Events
{
    public class TeamPrefChange : EventMessageBase
    {
        public string Name { get; set; }
        public bool Value { get; set; }
    }
}

