using System.Collections.Generic;
using Tab.Slack.Common.Model.Events.Messages;

namespace Tab.Slack.Common.Model
{
    /// <summary>
    /// Contains information about a Slack group.
    /// More information can be found at https://api.slack.com/types/group.
    /// </summary>
    public class Group : FlexibleJsonModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsGroup { get; set; }
        public long Created { get; set; }
        public string Creator { get; set; }
        public bool IsArchived { get; set; }
        public List<string> Members { get; set; }
        public Topic Topic { get; set; }
        public Purpose Purpose { get; set; }

        public bool IsOpen { get; set; }
        public string LastRead { get; set; }
        public int UnreadCount { get; set; }
        public MessageBase Latest { get; set; }
    }
}
