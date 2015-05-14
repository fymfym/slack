using Moq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model.Events;
using Xunit;

namespace Tab.Slack.WebApi.Tests
{
    public class SlackClientTests
    {
        [Fact]
        public void RtmStartShouldReturnEvent()
        {
            var content = @"{""ok"":true,""url"":""https://www.google.com/""}";

            IRestRequest requestMade = null;
            var mockRestClient = SetupMockRestClient(content, r => requestMade = r);
            var slackClient = SetupSlackClient(mockRestClient);

            var result = slackClient.RtmStart();

            mockRestClient.Verify();
            Assert.Equal(EventType.RtmStart, result.Type);
            Assert.Equal("/rtm.start", requestMade.Resource);
            Assert.Equal("https://www.google.com/", result.Url);
        }

        private SlackClient SetupSlackClient(Mock<IRestClient> restClient)
        {
            return new SlackClient("mockkey")
            {
                RestClient = restClient.Object
            };
        }

        private Mock<IRestClient> SetupMockRestClient(string contentToReturn, Action<IRestRequest> requestCallback = null)
        {
            var mockResponse = new Mock<IRestResponse>();
            mockResponse.Setup(r => r.Content)
                        .Returns(contentToReturn)
                        .Verifiable();

            var mockRestClient = new Mock<IRestClient>();
            mockRestClient.Setup(c => c.Execute(It.IsAny<IRestRequest>()))
                          .Callback<IRestRequest>(r =>
                          {
                              if (requestCallback != null)
                                  requestCallback.Invoke(r);
                          })
                          .Returns(mockResponse.Object)
                          .Verifiable();

            return mockRestClient;
        }
    }
}
