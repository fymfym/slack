using Moq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model.Events;
using Tab.Slack.Common.Model.Events.Messages;
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
            Assert.Equal("foo", result.Channel.Name);
        }

        [Fact]
        public void ChannelJoinShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""channel"":{""name"":""foo""}}");

            var result = context.SlackClient.ChannelJoin("foo");

            context.VerifyOk(result);
            Assert.Equal("/channels.join?name=foo", context.RequestMade.Resource);
            Assert.Equal("foo", result.Channel.Name);
        }

        [Fact]
        public void ChannelHistoryShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""messages"":[{""type"":""message"",""text"":""hello""},{""type"":""message"",""subtype"":""me_message"",""text"":""me""}]}");

            var result = context.SlackClient.ChannelHistory("foo", messageCount: 44);

            context.VerifyOk(result);
            Assert.Equal("/channels.history?channel=foo&inclusive=0&count=44", context.RequestMade.Resource);
            Assert.IsType<PlainMessage>(result.Messages[0]);
            Assert.Equal(MessageSubType.PlainMessage, result.Messages[0].Subtype);
            Assert.Equal("hello", result.Messages[0].Text);
            Assert.IsType<MeMessage>(result.Messages[1]);
            Assert.Equal(MessageSubType.MeMessage, result.Messages[1].Subtype);
        }

        [Fact]
        public void ChannelInfoShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""channel"":{""name"":""foo""}}");

            var result = context.SlackClient.ChannelInfo("foo");

            context.VerifyOk(result);
            Assert.Equal("/channels.info?channel=foo", context.RequestMade.Resource);
            Assert.Equal("foo", result.Channel.Name);
        }

        [Fact]
        public void ChannelInviteShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""channel"":{""name"":""foo""}}");

            var result = context.SlackClient.ChannelInvite("foo", "uid");

            context.VerifyOk(result);
            Assert.Equal("/channels.invite?channel=foo&user=uid", context.RequestMade.Resource);
            Assert.Equal("foo", result.Channel.Name);
        }

        [Fact]
        public void ChannelKickShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true}");

            var result = context.SlackClient.ChannelKick("CHANID", "UID");

            context.VerifyOk(result);
            Assert.Equal("/channels.kick?channel=CHANID&user=UID", context.RequestMade.Resource);
        }

        [Fact]
        public void ChannelLeaveShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""not_in_channel"":true}");

            var result = context.SlackClient.ChannelLeave("CHANID");

            context.VerifyOk(result);
            Assert.Equal("/channels.leave?channel=CHANID", context.RequestMade.Resource);
            Assert.True(result.NotInChannel);
        }

        [Fact]
        public void ChannelsListShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""channels"":[{""id"":""CHANID""}]}");

            var result = context.SlackClient.ChannelsList();

            context.VerifyOk(result);
            Assert.Equal("/channels.list?exclude_archived=0", context.RequestMade.Resource);
            Assert.Equal("CHANID", result.Channels[0].Id);
        }

        [Fact]
        public void ChannelMarkShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true}");

            var result = context.SlackClient.ChannelMark("CHANID","1111.2222");

            context.VerifyOk(result);
            Assert.Equal("/channels.mark?channel=CHANID&ts=1111.2222", context.RequestMade.Resource);
        }

        [Fact]
        public void ChannelRenameShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""channel"":{""name"":""foo""}}");

            var result = context.SlackClient.ChannelRename("foo", "uid");

            context.VerifyOk(result);
            Assert.Equal("/channels.rename?channel=foo&name=uid", context.RequestMade.Resource);
            Assert.Equal("foo", result.Channel.Name);
        }

        [Fact]
        public void ChannelSetPurposeShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""purpose"":""hello""}");

            var result = context.SlackClient.ChannelSetPurpose("foo", "hello");

            context.VerifyOk(result);
            Assert.Equal("/channels.setPurpose?channel=foo&purpose=hello", context.RequestMade.Resource);
            Assert.Equal("hello", result.Purpose);
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
