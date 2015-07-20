using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tab.Slack.Common.Model
{
    // todo: a bit of a catch-all class - needs rethinking
    public class ReactionItem : FlexibleJsonModel
    {
        public string Channel { get; set; }
        public string Ts { get; set; }
        public ReactionType Type { get; set; }
        public ReactionMessage Message { get; set; }
        public File File { get; set; }
        public ItemComment Comment { get; set; }
        public List<ReactionItem> Reactions { get; set; }
    }
}
