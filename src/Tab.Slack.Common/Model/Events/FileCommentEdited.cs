using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tab.Slack.Common.Model.Events
{
    public class FileCommentEdited : EventMessageBase
    {
        public File File { get; set; }
        public ItemComment Comment { get; set; }
        public string EventTs { get; set; }
    }
}

