using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tab.Slack.Common.Model.Events
{
    public class GroupJoined : EventMessageBase
    {
        public Channel Channel { get; set; }
    }
}

