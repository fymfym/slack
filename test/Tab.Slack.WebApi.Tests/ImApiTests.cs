using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model.Responses;
using Xunit;

namespace Tab.Slack.WebApi.Tests
{
    public class ImApiTests : ApiTestBase
    {
        [Fact]
        public void ImCloseShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<CloseResponse>("/im.close?channel=IMID");

            var subject = new ImApi(requestHandlerMock.Object);
            var result = subject.Close("IMID");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void ImHistoryShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<MessagesResponse>("/im.history?channel=foo&inclusive=0&count=44");

            var subject = new ImApi(requestHandlerMock.Object);
            var result = subject.History("foo", messageCount: 44);

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void ImListShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<ImsResponse>("/im.list?exclude_archived=0");

            var subject = new ImApi(requestHandlerMock.Object);
            var result = subject.List();

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void ImMarkShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<ResponseBase>("/im.mark?channel=IMID&ts=1111.2222");

            var subject = new ImApi(requestHandlerMock.Object);
            var result = subject.Mark("IMID", "1111.2222");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void ImOpenShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<ImOpenResponse>("/im.open?user=UID");

            var subject = new ImApi(requestHandlerMock.Object);
            var result = subject.Open("UID");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }
    }
}
