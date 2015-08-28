using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model.Responses;
using Xunit;

namespace Tab.Slack.WebApi.Tests
{
    public class TeamApiTests : ApiTestBase
    {
        [Fact]
        public void TeamAccessLogsShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<TeamAccessLogs>("/team.accessLogs?");

            var subject = new TeamApi(requestHandlerMock.Object);
            var result = subject.AccessLogs();

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void TeamInfoShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = ExecRequestMock<TeamResponse>("/team.info");

            var subject = new TeamApi(requestHandlerMock.Object);
            var result = subject.Info();

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }
    }
}
