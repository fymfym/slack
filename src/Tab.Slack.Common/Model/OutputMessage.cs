namespace Tab.Slack.Common.Model
{
    public class OutputMessage
    {
        public long Id { get; set; }
        public string Type { get { return "message"; } }
        public string Channel { get; set; }
        public string Text { get; set; }

        public OutputMessage(long id, string channel, string text)
        {
            Id = id;
            Channel = channel;
            Text = text;
        }
    }
}
