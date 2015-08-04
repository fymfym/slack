using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model;
using Tab.Slack.Common.Model.Events;
using Tab.Slack.Common.Model.Requests;
using Tab.Slack.Common.Model.Responses;
using Xunit;

namespace Tab.Slack.WebApi.Tests
{
    public class AuthApiTests : ApiTestBase
    {
        [Fact]
        public void AuthTestShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = ExecRequestMock<AuthTestResponse>("/auth.test");

            var subject = new AuthApi(requestHandlerMock.Object);
            var result = subject.Test();

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }
    }
}
