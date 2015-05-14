using Moq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model.Events;
using Tab.Slack.Common.Model.Responses;
using Xunit;

namespace Tab.Slack.WebApi.Tests
{
    public class SlackClientTests
    {
        [Fact]
        public void RtmStartShouldReturnEvent()
        {
            var context = SetupTestContext(@"{""ok"":true,""url"":""https://www.google.com/""}");
            
            var result = context.SlackClient.RtmStart();

            context.MockRestClient.Verify();
            Assert.Equal(EventType.RtmStart, result.Type);
            Assert.Equal("/rtm.start", context.RequestMade.Resource);
            Assert.Equal("https://www.google.com/", result.Url);
        }

        [Fact]
        public void ApiTestShouldReturnArgs()
        {
            var context = SetupTestContext(@"{""ok"":true,""args"":{""arg1"":""test""}}");

            var result = context.SlackClient.ApiTest(null, "test");

            context.VerifyOk(result);
            Assert.StartsWith("/api.test?arg1=test", context.RequestMade.Resource);
            Assert.Equal("test", result.Args["arg1"]);
        }

        [Fact]
        public void AuthTestShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""user_id"":""test""}");

            var result = context.SlackClient.AuthTest();

            context.VerifyOk(result);
            Assert.Equal("/auth.test", context.RequestMade.Resource);
            Assert.Equal("test", result.UserId);
        }

        [Fact]
        public void ChannelArchiveShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true}");

            var result = context.SlackClient.ChannelArchive("CHANID");

            context.VerifyOk(result);
            Assert.Equal("/channels.archive?channel=CHANID", context.RequestMade.Resource);
        }

        [Fact]
        public void ChannelCreateShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""channel"":{""name"":""foo""}}");

            var result = context.SlackClient.ChannelCreate("foo");

            context.VerifyOk(result);
            Assert.Equal("/channels.create?name=foo", context.RequestMade.Resource);
        }

        internal class TestContext
        {
            internal SlackClient SlackClient { get; set; }
            internal IRestRequest RequestMade { get; set; }
            internal Mock<IRestClient> MockRestClient { get; set; }
            internal string Content { get; set; }

            internal void VerifyOk(ResponseBase response)
            {
                this.MockRestClient.Verify();
                Assert.True(response.Ok);
            }
        }

        private TestContext SetupTestContext(string content)
        {
            var context = new TestContext(); 
            context.MockRestClient = SetupMockRestClient(content, r => context.RequestMade = r);
            context.Content = content;

            context.SlackClient = new SlackClient("mockkey")
            {
                RestClient = context.MockRestClient.Object
            };

            return context;
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
