using System.Threading.Tasks;
using Tab.Slack.Common.Model.Events;

namespace Tab.Slack.Bot.Integration
{
    public interface IMessageHandler
    {
        HandlerPriority Priority { get; }
        bool CanHandle(EventMessageBase message);
        Task<ProcessingChainResult> HandleMessageAsync(EventMessageBase message);
    }
}