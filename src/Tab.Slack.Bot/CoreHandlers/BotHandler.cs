using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model.Events;
using Tab.Slack.Common.Model.Events.Messages;
using Tab.Slack.Bot.Integration;
using Tab.Slack.Common.Model;

namespace Tab.Slack.Bot.CoreHandlers
{
    [Export(typeof(IMessageHandler))]
    public class BotHandler : MessageHandlerBase, IMessageHandler
    {
        public bool CanHandle(EventMessageBase message)
        {
            return message.IsOneOf(EventType.BotAdded, EventType.BotChanged);
        }

        public Task<ProcessingChainResult> HandleMessageAsync(EventMessageBase message)
        {
            var botModel = (message as BotAdded)?.Bot ?? (message as BotChanged)?.Bot;

            if (botModel != null)
            {
                var existingBotIndex = base.BotState.Bots.FindIndex(b => b.Id == botModel.Id);

                if (existingBotIndex >= 0)
                    base.BotState.Bots.RemoveAt(existingBotIndex);

                base.BotState.Bots.Add(botModel);
            }

            return Task.FromResult(ProcessingChainResult.Continue);
        }
    }
}
