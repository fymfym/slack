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
    public class ChannelHandler : MessageHandlerBase, IMessageHandler
    {
        public bool CanHandle(EventMessageBase message)
        {
            return message.IsOneOf(EventType.ChannelArchive, 
                                   EventType.ChannelCreated,
                                   EventType.ChannelDeleted,
                                   EventType.ChannelJoined,
                                   EventType.ChannelLeft,
                                   EventType.ChannelMarked,
                                   EventType.ChannelRename,
                                   EventType.ChannelUnarchive);
        }

        public Task<ProcessingChainResult> HandleMessageAsync(EventMessageBase message)
        {
            switch (message.Type)
            {
                case EventType.ChannelArchive:
                    ChannelArchive(message as ChannelArchive); break;
                case EventType.ChannelCreated:
                    ChannelCreated(message as ChannelCreated); break;
                case EventType.ChannelDeleted:
                    ChannelDeleted(message as ChannelDeleted); break;
                case EventType.ChannelJoined:
                    ChannelJoined(message as ChannelJoined); break;
                case EventType.ChannelLeft:
                    ChannelLeft(message as ChannelLeft); break;
                case EventType.ChannelMarked:
                    ChannelMarked(message as ChannelMarked); break;
                case EventType.ChannelRename:
                    ChannelRename(message as ChannelRename); break;
                case EventType.ChannelUnarchive:
                    ChannelUnarchive(message as ChannelUnarchive); break;

            }

            return Task.FromResult(ProcessingChainResult.Continue);
        }

        private void ChannelArchive(ChannelArchive message)
        {
            var channel = base.BotState.Channels.FirstOrDefault(c => c.Id == message.Channel);

            if (channel == null)
                return;

            channel.IsArchived = true;
        }

        private void ChannelCreated(ChannelCreated message)
        {
            var existingChannelIndex = base.BotState.Channels.FindIndex(c => c.Id == message.Channel.Id);

            if (existingChannelIndex >= 0)
                base.BotState.Channels.RemoveAt(existingChannelIndex);

            base.BotState.Channels.Add(message.Channel);
        }

        private void ChannelDeleted(ChannelDeleted message)
        {
            var existingChannelIndex = base.BotState.Channels.FindIndex(c => c.Id == message.Channel);

            if (existingChannelIndex >= 0)
                base.BotState.Channels.RemoveAt(existingChannelIndex);
        }

        private void ChannelJoined(ChannelJoined message)
        {
            var createdMessage = new ChannelCreated
            {
                Channel = message.Channel
            };

            ChannelCreated(createdMessage);
        }

        private void ChannelLeft(ChannelLeft message)
        {
            var channel = base.BotState.Channels.FirstOrDefault(c => c.Id == message.Channel);

            if (channel == null)
                return;

            channel.IsMember = false;
        }

        private void ChannelMarked(ChannelMarked message)
        {
            var channel = base.BotState.Channels.FirstOrDefault(c => c.Id == message.Channel);

            if (channel == null)
                return;

            channel.LastRead = message.Ts;
        }

        private void ChannelRename(ChannelRename message)
        {
            var channel = base.BotState.Channels.FirstOrDefault(c => c.Id == message.Channel.Id);

            if (channel == null)
                return;

            channel.Name = message.Channel.Name;
        }

        private void ChannelUnarchive(ChannelUnarchive message)
        {
            var channel = base.BotState.Channels.FirstOrDefault(c => c.Id == message.Channel);

            if (channel == null)
                return;

            channel.IsArchived = false;
        }
    }
}
