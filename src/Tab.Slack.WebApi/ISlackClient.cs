using Tab.Slack.Common.Json;
using Tab.Slack.Common.Model;
using Tab.Slack.Common.Model.Responses;

namespace Tab.Slack.WebApi
{
    public interface ISlackClient
    {
        IResponseParser ResponseParser { get; set; }

        RtmStartResponse RtmStart();
        TestResponse ApiTest(string error = null, params string[] args);
    }
}