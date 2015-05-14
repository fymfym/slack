using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tab.Slack.Common.Model.Events
{
    public class GroupArchive : EventMessageBase
    {
        public string Channel { get; set; }
    }
}

