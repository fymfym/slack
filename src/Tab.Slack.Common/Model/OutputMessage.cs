namespace Tab.Slack.Common.Model
{
    public class OutputMessage
    {
        public long Id { get; set; }
        public string Type { get; set; } = "message";
        public string Channel { get; set; }
        public string Text { get; set; }
    }
}
