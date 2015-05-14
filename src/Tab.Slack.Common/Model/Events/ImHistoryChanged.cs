using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tab.Slack.Common.Model.Events
{
    public class ImHistoryChanged : EventMessageBase
    {
        public string Latest { get; set; }
        public string Ts { get; set; }
        public string EventTs { get; set; }
    }
}

