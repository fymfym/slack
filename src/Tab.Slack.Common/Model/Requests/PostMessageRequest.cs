using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tab.Slack.Common.Model.Requests
{
    public class PostMessageRequest
    {
        public string Channel { get; set; }
        public string Text { get; set; }
        public string Username { get; set; }
        public bool? AsUser { get; set; }
        public ParseMode? Parse { get; set; }
        public int? LinkNames { get; set; }
        public List<Attachment> Attachments { get; set; }
        public bool? UnfurlLinks { get; set; }
        public bool? UnfurlMedia { get; set; }
        public string IconUrl { get; set; }
        public string IconEmoji { get; set; }
    }
}
