using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tab.Slack.Common.Model
{
    public class SlackBot : FlexibleJsonModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public BotIcons Icons { get; set; }
    }
}
