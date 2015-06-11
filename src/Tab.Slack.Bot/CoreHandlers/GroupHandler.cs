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
    public class GroupHandler : MessageHandlerBase, IMessageHandler
    {
        public bool CanHandle(EventMessageBase message)
        {
            return message.IsOneOf(EventType.GroupArchive, 
                                   EventType.GroupJoined,
                                   EventType.GroupLeft,
                                   EventType.GroupMarked,
                                   EventType.GroupRename,
                                   EventType.GroupUnarchive);
        }

        public Task<ProcessingChainResult> HandleMessageAsync(EventMessageBase message)
        {
            switch (message.Type)
            {
                case EventType.GroupArchive:
                    GroupArchive(message as GroupArchive); break;
                case EventType.GroupJoined:
                    GroupJoined(message as GroupJoined); break;
                case EventType.GroupLeft:
                    GroupLeft(message as GroupLeft); break;
                case EventType.GroupMarked:
                    GroupMarked(message as GroupMarked); break;
                case EventType.GroupRename:
                    GroupRename(message as GroupRename); break;
                case EventType.GroupUnarchive:
                    GroupUnarchive(message as GroupUnarchive); break;
            }

            return Task.FromResult(ProcessingChainResult.Continue);
        }

        private void GroupArchive(GroupArchive message)
        {
            var group = base.BotState.Groups.FirstOrDefault(c => c.Id == message.Channel);

            if (group == null)
                return;

            group.IsArchived = true;
        }

        private void GroupJoined(GroupJoined message)
        {
            var existingGroupIndex = base.BotState.Groups.FindIndex(c => c.Id == message.Channel.Id);

            if (existingGroupIndex >= 0)
                base.BotState.Groups.RemoveAt(existingGroupIndex);

            base.BotState.Groups.Add(message.Channel);
        }

        private void GroupLeft(GroupLeft message)
        {
            var group = base.BotState.Groups.FirstOrDefault(c => c.Id == message.Channel.Id);

            if (group == null)
                return;

            group.IsOpen = false;
        }

        private void GroupMarked(GroupMarked message)
        {
            var group = base.BotState.Groups.FirstOrDefault(c => c.Id == message.Channel);

            if (group == null)
                return;

            group.LastRead = message.Ts;
        }

        private void GroupRename(GroupRename message)
        {
            var group = base.BotState.Groups.FirstOrDefault(c => c.Id == message.Channel.Id);

            if (group == null)
                return;

            group.Name = message.Channel.Name;
        }

        private void GroupUnarchive(GroupUnarchive message)
        {
            var group = base.BotState.Groups.FirstOrDefault(c => c.Id == message.Channel);

            if (group == null)
                return;

            group.IsArchived = false;
        }
    }
}
