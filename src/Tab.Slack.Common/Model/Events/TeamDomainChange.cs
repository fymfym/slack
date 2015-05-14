using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tab.Slack.Common.Model.Events
{
    public class TeamDomainChange : EventMessageBase
    {
        public string Url { get; set; }
        public string Domain { get; set; }
    }
}

