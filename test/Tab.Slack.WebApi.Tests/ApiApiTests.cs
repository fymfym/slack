using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model.Requests;
using Tab.Slack.Common.Model.Responses;
using Xunit;

namespace Tab.Slack.WebApi.Tests
{
    public class ApiApiTests : ApiTestBase
    {
        [Fact]
        public void ApiTestShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = ExecRequestMock<ApiTestResponse>("/api.test?arg1=testx");
            
            var subject = new ApiApi(requestHandlerMock.Object);
            var result = subject.Test(null, "testx");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }
    }
}
