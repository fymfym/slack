using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tab.Slack.Common.Model.Events;

namespace Tab.Slack.Common.Model
{
    public class EditedMessage : FlexibleJsonModel
    {
        public EventType Type { get; set; }
        public string User { get; set; }
        public string Text { get; set; }
        public string Ts { get; set; }
        public MessageEditor Edited { get; set; }
    }
}
