using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tab.Slack.Common.Model.Events.Messages
{
    public class FileComment : MessageBase
    {
        public File File { get; set; }
        public ItemComment Comment { get; set; }
    }
}

