using Tab.Slack.Common.Json;
using Tab.Slack.Common.Model;

namespace Tab.Slack.Rest
{
    public interface ISlackClient
    {
        IResponseParser ResponseParser { get; set; }

        RtmStartResponse RtmStart();
    }
}