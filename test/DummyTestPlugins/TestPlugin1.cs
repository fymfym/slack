using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Bot.Integration;
using Tab.Slack.Common.Model.Events;

namespace DummyTestPlugins
{
    [Export(typeof(IMessageHandler))]
    public class TestPlugin1 : MessageHandlerBase, IMessageHandler
    {
        public bool CanHandle(EventMessageBase message)
        {
            throw new NotImplementedException();
        }

        public Task<ProcessingChainResult> HandleMessageAsync(EventMessageBase message)
        {
            throw new NotImplementedException();
        }
    }
}
