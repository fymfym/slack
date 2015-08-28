using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model.Responses;
using Xunit;

namespace Tab.Slack.WebApi.Tests
{
    public class OauthApiTests : ApiTestBase
    {
        [Fact]
        public void OauthAccessShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<OauthAccessResponse>("/oauth.access?client_id=CLIENTID&client_secret=SECRET&code=XXX");

            var subject = new OauthApi(requestHandlerMock.Object);
            var result = subject.Access("CLIENTID", "SECRET", "XXX");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }
    }
}
