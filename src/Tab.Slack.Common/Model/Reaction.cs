using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tab.Slack.Common.Model.Events.Messages;

namespace Tab.Slack.Common.Model
{
    public class Reaction : FlexibleJsonModel
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public List<string> Users { get; set; }
    }
}
