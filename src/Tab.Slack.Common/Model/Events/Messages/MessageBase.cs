using System.Collections.Generic;

namespace Tab.Slack.Common.Model.Events.Messages
{
    public class MessageBase : EventMessageBase
    {
        public MessageSubType Subtype { get; set; }
        public string Ts { get; set; }
        public string Text { get; set; }
        public string User { get; set; }
        public string Channel { get; set; }
        public string Team { get; set; }
        public List<Attachment> Attachments { get; set; }
        public bool Mrkdwn { get; set; }
    }
}
