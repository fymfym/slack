using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tab.Slack.Common.Model.Events
{
    public class ImCreated : EventMessageBase
    {
        public string User { get; set; }
        public Channel Channel { get; set; }
    }
}

