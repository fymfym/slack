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
    public class TeamHandler : MessageHandlerBase, IMessageHandler
    {
        public bool CanHandle(EventMessageBase message)
        {
            return message.IsOneOf(EventType.PrefChange, 
                                   EventType.UserChange,
                                   EventType.TeamJoin,
                                   EventType.TeamPrefChange,
                                   EventType.TeamRename,
                                   EventType.TeamDomainChange,
                                   EventType.EmailDomainChanged);
        }

        public Task<ProcessingChainResult> HandleMessageAsync(EventMessageBase message)
        {
            switch (message.Type)
            {
                case EventType.PrefChange:
                    PrefChange(message as PrefChange); break;
                case EventType.UserChange:
                    UserChange(message as UserChange); break;
                case EventType.TeamJoin:
                    TeamJoin(message as TeamJoin); break;
                case EventType.TeamPrefChange:
                    TeamPrefChange(message as TeamPrefChange); break;
                case EventType.TeamDomainChange:
                    TeamDomainChange(message as TeamDomainChange); break;
                case EventType.TeamRename:
                    TeamRename(message as TeamRename); break;
                case EventType.EmailDomainChanged:
                    EmailDomainChanged(message as EmailDomainChanged); break;
            }

            return Task.FromResult(ProcessingChainResult.Continue);
        }

        private void PrefChange(PrefChange message)
        {
            this.BotState.Self.Prefs[message.Name] = message.Value;
        }

        private void UserChange(UserChange message)
        {
            var existingUserIndex = base.BotState.Users.FindIndex(i => i.Id == message.User.Id);

            if (existingUserIndex >= 0)
                base.BotState.Users.RemoveAt(existingUserIndex);

            base.BotState.Users.Add(message.User);
        }

        private void TeamJoin(TeamJoin message)
        {
            var existingUserIndex = base.BotState.Users.FindIndex(i => i.Id == message.User.Id);

            if (existingUserIndex >= 0)
                base.BotState.Users.RemoveAt(existingUserIndex);

            base.BotState.Users.Add(message.User);
        }

        private void TeamPrefChange(TeamPrefChange message)
        {
            this.BotState.Team.Prefs[message.Name] = message.Value;
        }

        private void TeamDomainChange(TeamDomainChange message)
        {
            this.BotState.Team.Domain = message.Domain;
        }

        private void TeamRename(TeamRename message)
        {
            this.BotState.Team.Name = message.Name;
        }

        private void EmailDomainChanged(EmailDomainChanged message)
        {
            this.BotState.Team.EmailDomain = message.EmailDomain;
        }
    }
}
