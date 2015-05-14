namespace Tab.Slack.Common.Model
{
    public class Purpose : FlexibleJsonModel
    {
        public string Value { get; set; }
        public string Creator { get; set; }

        /// <summary>
        /// The date the purpose was last updated (Unix timestamp).
        /// </summary>
        public long LastSet { get; set; }
    }
}