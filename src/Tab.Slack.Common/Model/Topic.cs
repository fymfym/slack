namespace Tab.Slack.Common.Model
{
    public class Topic : FlexibleJsonModel
    {
        public string Value { get; set; }
        public string Creator { get; set; }

        /// <summary>
        /// The date the topic was last updated (Unix timestamp).
        /// </summary>
        public long LastSet { get; set; }
    }
}