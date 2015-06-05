using Tab.Slack.Common.Model.Events.Messages;

namespace Tab.Slack.Common.Model.Responses
{
    public class StarItemResponse
    {
        public StarItemType Type { get; set; }
        public string Channel { get; set; }
        public MessageBase Message { get; set; }
        public File File { get; set; }
        public FileComment Comment { get; set; }
        public string Group { get; set; }
    }
}