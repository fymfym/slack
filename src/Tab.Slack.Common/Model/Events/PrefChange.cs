using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tab.Slack.Common.Model.Events
{
    public class PrefChange : EventMessageBase
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}

