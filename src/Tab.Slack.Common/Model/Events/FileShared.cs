using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tab.Slack.Common.Model.Events
{
    public class FileShared : EventMessageBase
    {
        public File File { get; set; }
        public string EventTs { get; set; }
    }
}

