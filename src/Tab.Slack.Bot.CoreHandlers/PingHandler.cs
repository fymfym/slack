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
    public class PingHandler : MessageHandlerBase, IMessageHandler
    {
        public bool CanHandle(EventMessageBase message)
        {
            return message.IsToMe(base.BotState)
                && message.MatchesText(@"\bping\b");
        }

        public Task<ProcessingChainResult> HandleMessageAsync(EventMessageBase message)
        {
            var messageBase = message.CastTo<MessageBase>();

            var response = "pong";

            if (!message.IsIm(base.BotState))
                response = $"<@{messageBase.User}>: {response}";

            base.BotServices.SendMessage(messageBase.Channel, response);

            return Task.FromResult(ProcessingChainResult.Continue);
        }
    }
}
