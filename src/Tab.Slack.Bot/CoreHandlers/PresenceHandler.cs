using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model.Events;
using Tab.Slack.Common.Model.Events.Messages;
using Tab.Slack.Bot.Integration;

namespace Tab.Slack.Bot.CoreHandlers
{
    [Export(typeof(IMessageHandler))]
    public class PresenceHandler : MessageHandlerBase, IMessageHandler
    {
        public bool CanHandle(EventMessageBase message)
        {
            // TODO: EventType.ManualPresenceChange ?
            return message.IsOneOf(EventType.PresenceChange);
        }

        public Task<ProcessingChainResult> HandleMessageAsync(EventMessageBase message)
        {
            var presenceMessage = message.CastTo<PresenceChange>();

            var user = base.BotState.Users.FirstOrDefault(u => u.Id == presenceMessage.User);

            if (user != null)
                user.Presence = presenceMessage.Presence;

            return Task.FromResult(ProcessingChainResult.Continue);
        }
    }
}
