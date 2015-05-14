using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tab.Slack.Common.Model.Events.Messages;

namespace Tab.Slack.Common.Model
{
    public class Item : FlexibleJsonModel
    {
        public ItemType Type { get; set; }
        public string Channel { get; set; }
        public File File { get; set; }
        public MessageBase Message { get; set; }
        public string Comment { get; set; }
    }
}
