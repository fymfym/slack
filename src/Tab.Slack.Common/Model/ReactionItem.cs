using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model.Responses;

namespace Tab.Slack.Common.Model
{
    // todo: a bit of a catch-all class - needs rethinking
    public class ReactionItem : ResponseBase
    {
        public string Channel { get; set; }
        public string Ts { get; set; }
        public ReactionType Type { get; set; }
        public ReactionMessage Message { get; set; }
        public File File { get; set; }
        public ItemComment Comment { get; set; }
        public List<Reaction> Reactions { get; set; }
    }
}
