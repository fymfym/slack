using System.Collections.Generic;
using Tab.Slack.Common.Model.Events.Messages;

namespace Tab.Slack.Common.Model
{
    // todo: this class is a hack (I blame Slack's inconsistent schema) - needs rethinking 
    public class ReactionMessage : MessageBase
    {
        public List<Reaction> Reactions { get; set; }
    }
}