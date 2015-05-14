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

        [Fact]
        public void ApiTestShouldReturnArgs()
        {
            var content = @"{""ok"":true,""args"":{""arg1"":""test""}}";

            IRestRequest requestMade = null;
            var mockRestClient = SetupMockRestClient(content, r => requestMade = r);
            var slackClient = SetupSlackClient(mockRestClient);

            var result = slackClient.ApiTest(null, "test");

            mockRestClient.Verify();
            Assert.True(result.Ok);
            Assert.StartsWith("/api.test", requestMade.Resource);
            Assert.Equal("test", result.Args["arg1"]);
        }

        [Fact]
        public void AuthTestShouldReturnResponse()
        {
            var content = @"{""ok"":true,""user_id"":""test""}";

            IRestRequest requestMade = null;
            var mockRestClient = SetupMockRestClient(content, r => requestMade = r);
            var slackClient = SetupSlackClient(mockRestClient);

            var result = slackClient.AuthTest();

            mockRestClient.Verify();
            Assert.True(result.Ok);
            Assert.Equal("/auth.test", requestMade.Resource);
            Assert.Equal("test", result.UserId);
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
