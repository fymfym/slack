using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tab.Slack.Common.Model.Events
{
    public class FileDeleted : EventMessageBase
    {
        public string FileId { get; set; }
        public string EventTs { get; set; }
    }
}

