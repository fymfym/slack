namespace Tab.Slack.Common.Model.Events.Messages
{
    public abstract class MessageBase : EventMessageBase
    {
        public MessageSubType Subtype { get; set; }
        public string Ts { get; set; }
        public string Text { get; set; }
        public string User { get; set; }
        public string Channel { get; set; }
        public string Team { get; set; }
    }
}