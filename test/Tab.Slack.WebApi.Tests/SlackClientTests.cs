using Moq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model;
using Tab.Slack.Common.Model.Events;
using Tab.Slack.Common.Model.Events.Messages;
using Tab.Slack.Common.Model.Requests;
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

        [Fact]
        public void ChannelSetTopicShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""topic"":""hello""}");

            var result = context.SlackClient.ChannelSetTopic("foo", "hello");

            context.VerifyOk(result);
            Assert.Equal("/channels.setTopic?channel=foo&topic=hello", context.RequestMade.Resource);
            Assert.Equal("hello", result.Topic);
        }

        [Fact]
        public void ChannelUnarchiveShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true}");

            var result = context.SlackClient.ChannelUnarchive("foo");

            context.VerifyOk(result);
            Assert.Equal("/channels.unarchive?channel=foo", context.RequestMade.Resource);
        }

        [Fact]
        public void ChatDeleteShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""channel"":""foo"",""ts"":""1111.2222""}");

            var result = context.SlackClient.ChatDelete("foo", "1111.2222");

            context.VerifyOk(result);
            Assert.Equal("/chat.delete?channel=foo&ts=1111.2222", context.RequestMade.Resource);
            Assert.Equal("foo", result.Channel);
            Assert.Equal("1111.2222", result.Ts);
        }

        [Fact]
        public void ChatPostMessageShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""message"":{""type"":""message""}}");

            var request = new PostMessageRequest
            {
                Channel = "foo",
                Parse = ParseMode.Full,
                Attachments = new List<Attachment>
                {
                    new Attachment
                    {
                        Text = "Attach1",
                        Fields = new List<Field>
                        {
                            new Field { Title = "F1", Value = "V1" },
                            new Field { Title = "F2", Value = "V2" },
                        }
                    }
                }
            };

            var result = context.SlackClient.ChatPostMessage(request);

            context.VerifyOk(result);
            Assert.Equal("/chat.postMessage", context.RequestMade.Resource);
            Assert.True(context.RequestMade.Parameters.Any(p => p.Name == "parse" && (string)p.Value == "full"));
        }

        [Fact]
        public void ChatUpdateShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""channel"":""foo"",""text"":""bar""}");

            var result = context.SlackClient.ChatUpdate("foo", "1111.2222", "bar");

            context.VerifyOk(result);
            Assert.Equal("/chat.update?channel=foo&ts=1111.2222&text=bar", context.RequestMade.Resource);
            Assert.Equal("foo", result.Channel);
            Assert.Equal("bar", result.Text);
        }

        [Fact]
        public void EmojiListShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""emoji"":{""foo"":""bar""}}");

            var result = context.SlackClient.EmojiList();

            context.VerifyOk(result);
            Assert.Equal("/emoji.list", context.RequestMade.Resource);
            Assert.Equal("bar", result.Emoji["foo"]);
        }

        [Fact]
        public void FileDeleteShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true}");

            var result = context.SlackClient.FileDelete("foo");

            context.VerifyOk(result);
            Assert.Equal("/files.delete?file=foo", context.RequestMade.Resource);
        }

        internal class TestContext
        {
            internal ISlackClient SlackClient { get; set; }
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
