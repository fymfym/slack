using log4net;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tab.Slack.Bot.Integration;
using Tab.Slack.Common.Json;
using Tab.Slack.WebApi;

namespace Tab.Slack.Bot
{
    public interface ISlackBot : IDisposable
    {
        IEnumerable<IMessageHandler> MessageHandlers { get; set; }
        IBotState SlackState { get; set; }
        IBotServices SlackService { get; set; }
        IResponseParser ResponseParser { get; set; }
        ISlackApi SlackApi { get; set; }
        IBackOffStrategy BackOffStrategy { get; set; }
        ILog Logger { get; set; }

        bool AutoReconnect { get; set; }
        bool StrictProtocolWarnings { get; set; }

        Task Start();
        void Stop();
    }
}