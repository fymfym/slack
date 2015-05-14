using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tab.Slack.Common.Model.Events
{
    public class ManualPresenceChange : EventMessageBase
    {
        public string Presence { get; set; }
    }
}

