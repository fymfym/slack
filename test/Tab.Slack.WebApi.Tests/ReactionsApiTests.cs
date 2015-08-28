using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model;
using Tab.Slack.Common.Model.Responses;
using Xunit;

namespace Tab.Slack.WebApi.Tests
{
    public class ReactionsApiTests : ApiTestBase
    {
        [Fact]
        public void ReactionAddShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<ResponseBase>("/reactions.add?name=ok&channel=CHANID&timestamp=123.123");

            var subject = new ReactionsApi(requestHandlerMock.Object);
            var result = subject.Add("ok", channelId: "CHANID", ts: "123.123");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void ReactionGetShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<ReactionItem>("/reactions.get?channel=CHANID&timestamp=123.123");

            var subject = new ReactionsApi(requestHandlerMock.Object);
            var result = subject.Get(channelId: "CHANID", ts: "123.123");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void ReactionListShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<ReactionListResponse>("/reactions.list?user=USERID");

            var subject = new ReactionsApi(requestHandlerMock.Object);
            var result = subject.List(userId: "USERID");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void ReactionRemoveShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<ResponseBase>("/reactions.remove?name=ok&channel=CHANID&timestamp=123.123");

            var subject = new ReactionsApi(requestHandlerMock.Object);
            var result = subject.Remove("ok", channelId: "CHANID", ts: "123.123");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }
    }
}
