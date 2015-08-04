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
    public class RtmApiTests : ApiTestBase
    {
        [Fact]
        public void RtmStartShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = ExecRequestMock<RtmStartResponse>("/rtm.start");

            var subject = new RtmApi(requestHandlerMock.Object);
            var result = subject.Start();

            requestHandlerMock.Verify();
            Assert.NotNull(result);
            Assert.Equal(EventType.RtmStart, result.Type);
        }
    }
}
