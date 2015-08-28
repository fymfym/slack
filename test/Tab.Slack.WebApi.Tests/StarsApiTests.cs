using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model.Responses;
using Xunit;

namespace Tab.Slack.WebApi.Tests
{
    public class StarsApiTests : ApiTestBase
    {
        [Fact]
        public void StarsListShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<StarsResponse>("/stars.list?");

            var subject = new StarsApi(requestHandlerMock.Object);
            var result = subject.List();

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }
    }
}
