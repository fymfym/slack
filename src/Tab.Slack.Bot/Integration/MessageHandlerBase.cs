using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Tab.Slack.Common.Model.Events;
using Tab.Slack.Common.Model.Events.Messages;

namespace Tab.Slack.Bot.Integration
{
    public abstract class MessageHandlerBase
    {
        public virtual HandlerPriority Priority { get; } = HandlerPriority.Normal;

        [Import]
        public IBotState BotState { get; set; }

        [Import]
        public IBotServices BotServices { get; set; }

        [Import]
        public ILog Logger { get; set; }
    }
}
