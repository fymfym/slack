using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tab.Slack.Common.Model.Events
{
    public class UserChange : EventMessageBase
    {
        public User User { get; set; }
        public string CacheTs { get; set; }
    }
}

