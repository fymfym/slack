using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model.Events;
using Tab.Slack.Bot.Integration;
using Tab.Slack.Common.Model;

namespace Tab.Slack.Bot.CoreHandlers
{
    [Export(typeof(IMessageHandler))]
    public class RtmStartHandler : MessageHandlerBase, IMessageHandler
    {
        public bool CanHandle(EventMessageBase message)
        {
            return message.IsOneOf(EventType.RtmStart);
        }

        public Task<ProcessingChainResult> HandleMessageAsync(EventMessageBase message)
        {
            var rtmStartResponse = message.CastTo<RtmStartResponse>();

            base.BotState.Bots = rtmStartResponse.Bots;
            base.BotState.CacheVersion = rtmStartResponse.CacheVersion;
            base.BotState.Channels = rtmStartResponse.Channels;
            base.BotState.Groups = rtmStartResponse.Groups;
            base.BotState.Ims = rtmStartResponse.Ims;
            base.BotState.LatestEventTs = rtmStartResponse.LatestEventTs;
            base.BotState.Self = rtmStartResponse.Self;
            base.BotState.Team = rtmStartResponse.Team;
            base.BotState.Users = rtmStartResponse.Users;

            return Task.FromResult(ProcessingChainResult.Continue);
        }
    }
}
