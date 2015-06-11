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
    public class ImHandler : MessageHandlerBase, IMessageHandler
    {
        public bool CanHandle(EventMessageBase message)
        {
            return message.IsOneOf(EventType.ImClose, 
                                   EventType.ImCreated,
                                   EventType.ImMarked);
        }

        public Task<ProcessingChainResult> HandleMessageAsync(EventMessageBase message)
        {
            switch (message.Type)
            {
                case EventType.ImClose:
                    ImClose(message as ImClose); break;
                case EventType.ImCreated:
                    ImCreated(message as ImCreated); break;
                case EventType.ImMarked:
                    ImMarked(message as ImMarked); break;
            }

            return Task.FromResult(ProcessingChainResult.Continue);
        }

        private void ImCreated(ImCreated message)
        {
            var existingImIndex = base.BotState.Ims.FindIndex(i => i.Id == message.Channel.Id);

            if (existingImIndex >= 0)
                base.BotState.Ims.RemoveAt(existingImIndex);

            base.BotState.Ims.Add(message.Channel);
        }

        private void ImClose(ImClose message)
        {
            var existingImIndex = base.BotState.Ims.FindIndex(i => i.Id == message.Channel);

            if (existingImIndex >= 0)
                base.BotState.Ims.RemoveAt(existingImIndex);
        }

        private void ImMarked(ImMarked message)
        {
            var im = base.BotState.Ims.FirstOrDefault(i => i.Id == message.Channel);

            if (im == null)
                return;

            im.LastRead = message.Ts;
        }
    }
}
