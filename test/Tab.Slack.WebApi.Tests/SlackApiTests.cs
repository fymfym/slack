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
    // TODO: refactor this mess
    public class SlackApiTests
    {
        [Fact]
        public void RtmStartShouldReturnEvent()
        {
            var context = SetupTestContext(@"{""ok"":true,""url"":""https://www.google.com/""}");
            
            var result = context.SlackApi.RtmStart();

            context.MockRestClient.Verify();
            Assert.Equal(EventType.RtmStart, result.Type);
            Assert.Equal("/rtm.start", context.RequestMade.Resource);
            Assert.Equal("https://www.google.com/", result.Url);
        }

        [Fact]
        public void ApiTestShouldReturnArgs()
        {
            var context = SetupTestContext(@"{""ok"":true,""args"":{""arg1"":""test""}}");

            var result = context.SlackApi.ApiTest(null, "test");

            context.VerifyOk(result);
            Assert.StartsWith("/api.test?arg1=test", context.RequestMade.Resource);
            Assert.Equal("test", result.Args["arg1"]);
        }

        [Fact]
        public void AuthTestShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""user_id"":""test""}");

            var result = context.SlackApi.AuthTest();

            context.VerifyOk(result);
            Assert.Equal("/auth.test", context.RequestMade.Resource);
            Assert.Equal("test", result.UserId);
        }

        [Fact]
        public void ChannelArchiveShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true}");

            var result = context.SlackApi.ChannelArchive("CHANID");

            context.VerifyOk(result);
            Assert.Equal("/channels.archive?channel=CHANID", context.RequestMade.Resource);
        }

        [Fact]
        public void ChannelCreateShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""channel"":{""name"":""foo""}}");

            var result = context.SlackApi.ChannelCreate("foo");

            context.VerifyOk(result);
            Assert.Equal("/channels.create?name=foo", context.RequestMade.Resource);
            Assert.Equal("foo", result.Channel.Name);
        }

        [Fact]
        public void ChannelJoinShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""channel"":{""name"":""foo""}}");

            var result = context.SlackApi.ChannelJoin("foo");

            context.VerifyOk(result);
            Assert.Equal("/channels.join?name=foo", context.RequestMade.Resource);
            Assert.Equal("foo", result.Channel.Name);
        }

        [Fact]
        public void ChannelHistoryShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""messages"":[{""type"":""message"",""text"":""hello""},{""type"":""message"",""subtype"":""me_message"",""text"":""me""}]}");

            var result = context.SlackApi.ChannelHistory("foo", messageCount: 44);

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

            var result = context.SlackApi.ChannelInfo("foo");

            context.VerifyOk(result);
            Assert.Equal("/channels.info?channel=foo", context.RequestMade.Resource);
            Assert.Equal("foo", result.Channel.Name);
        }

        [Fact]
        public void ChannelInviteShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""channel"":{""name"":""foo""}}");

            var result = context.SlackApi.ChannelInvite("foo", "uid");

            context.VerifyOk(result);
            Assert.Equal("/channels.invite?channel=foo&user=uid", context.RequestMade.Resource);
            Assert.Equal("foo", result.Channel.Name);
        }

        [Fact]
        public void ChannelKickShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true}");

            var result = context.SlackApi.ChannelKick("CHANID", "UID");

            context.VerifyOk(result);
            Assert.Equal("/channels.kick?channel=CHANID&user=UID", context.RequestMade.Resource);
        }

        [Fact]
        public void ChannelLeaveShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""not_in_channel"":true}");

            var result = context.SlackApi.ChannelLeave("CHANID");

            context.VerifyOk(result);
            Assert.Equal("/channels.leave?channel=CHANID", context.RequestMade.Resource);
            Assert.True(result.NotInChannel);
        }

        [Fact]
        public void ChannelsListShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""channels"":[{""id"":""CHANID""}]}");

            var result = context.SlackApi.ChannelList();

            context.VerifyOk(result);
            Assert.Equal("/channels.list?exclude_archived=0", context.RequestMade.Resource);
            Assert.Equal("CHANID", result.Channels[0].Id);
        }

        [Fact]
        public void ChannelMarkShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true}");

            var result = context.SlackApi.ChannelMark("CHANID","1111.2222");

            context.VerifyOk(result);
            Assert.Equal("/channels.mark?channel=CHANID&ts=1111.2222", context.RequestMade.Resource);
        }

        [Fact]
        public void ChannelRenameShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""channel"":{""name"":""foo""}}");

            var result = context.SlackApi.ChannelRename("foo", "uid");

            context.VerifyOk(result);
            Assert.Equal("/channels.rename?channel=foo&name=uid", context.RequestMade.Resource);
            Assert.Equal("foo", result.Channel.Name);
        }

        [Fact]
        public void ChannelSetPurposeShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""purpose"":""hello""}");

            var result = context.SlackApi.ChannelSetPurpose("foo", "hello");

            context.VerifyOk(result);
            Assert.Equal("/channels.setPurpose?channel=foo&purpose=hello", context.RequestMade.Resource);
            Assert.Equal("hello", result.Purpose);
        }

        [Fact]
        public void ChannelSetTopicShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""topic"":""hello""}");

            var result = context.SlackApi.ChannelSetTopic("foo", "hello");

            context.VerifyOk(result);
            Assert.Equal("/channels.setTopic?channel=foo&topic=hello", context.RequestMade.Resource);
            Assert.Equal("hello", result.Topic);
        }

        [Fact]
        public void ChannelUnarchiveShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true}");

            var result = context.SlackApi.ChannelUnarchive("foo");

            context.VerifyOk(result);
            Assert.Equal("/channels.unarchive?channel=foo", context.RequestMade.Resource);
        }

        [Fact]
        public void ChatDeleteShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""channel"":""foo"",""ts"":""1111.2222""}");

            var result = context.SlackApi.ChatDelete("foo", "1111.2222");

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

            var result = context.SlackApi.ChatPostMessage(request);

            context.VerifyOk(result);
            Assert.Equal("/chat.postMessage", context.RequestMade.Resource);
            Assert.True(context.RequestMade.Parameters.Any(p => p.Name == "parse" && (string)p.Value == "full"));
        }

        [Fact]
        public void ChatUpdateShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""channel"":""foo"",""text"":""bar""}");

            var result = context.SlackApi.ChatUpdate("foo", "1111.2222", "bar");

            context.VerifyOk(result);
            Assert.Equal("/chat.update?channel=foo&ts=1111.2222&text=bar", context.RequestMade.Resource);
            Assert.Equal("foo", result.Channel);
            Assert.Equal("bar", result.Text);
        }

        [Fact]
        public void EmojiListShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""emoji"":{""foo"":""bar""}}");

            var result = context.SlackApi.EmojiList();

            context.VerifyOk(result);
            Assert.Equal("/emoji.list", context.RequestMade.Resource);
            Assert.Equal("bar", result.Emoji["foo"]);
        }

        [Fact]
        public void FileDeleteShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true}");

            var result = context.SlackApi.FileDelete("foo");

            context.VerifyOk(result);
            Assert.Equal("/files.delete?file=foo", context.RequestMade.Resource);
        }

        [Fact]
        public void FileInfoShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""file"":{""id"":""foo""}}");

            var result = context.SlackApi.FileInfo("foo", 30);

            context.VerifyOk(result);
            Assert.Equal("/files.info?file=foo&count=30&page=1", context.RequestMade.Resource);
            Assert.Equal("foo", result.File.Id);
        }

        [Fact]
        public void FileListShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""files"":[{""id"":""foo""}]}");

            var request = new FilesListRequest { Types = "all" };
            var result = context.SlackApi.FileList(request);

            context.VerifyOk(result);
            Assert.Equal("/files.list", context.RequestMade.Resource);
            Assert.True(context.RequestMade.Parameters.Any(p => p.Name == "types" && (string)p.Value == "all"));
            Assert.Equal("foo", result.Files[0].Id);
        }

        [Fact]
        public void FileUploadShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""file"":{""id"":""foo""}}");

            var request = new FileUploadRequest {
                Filename = "hello.txt",
                FileData = Encoding.ASCII.GetBytes("hello world")
            };

            var result = context.SlackApi.FileUpload(request);

            context.VerifyOk(result);
            Assert.Equal("/files.upload", context.RequestMade.Resource);
            Assert.Equal("hello.txt", context.RequestMade.Files[0].FileName);
            Assert.Equal("foo", result.File.Id);
        }

        [Fact]
        public void GroupArchiveShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true}");

            var result = context.SlackApi.GroupArchive("GROUPID");

            context.VerifyOk(result);
            Assert.Equal("/groups.archive?channel=GROUPID", context.RequestMade.Resource);
        }

        [Fact]
        public void GroupCloseShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true}");

            var result = context.SlackApi.GroupClose("GROUPID");

            context.VerifyOk(result);
            Assert.Equal("/groups.close?channel=GROUPID", context.RequestMade.Resource);
        }

        [Fact]
        public void GroupCreateShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""group"":{""name"":""foo""}}");

            var result = context.SlackApi.GroupCreate("foo");

            context.VerifyOk(result);
            Assert.Equal("/groups.create?name=foo", context.RequestMade.Resource);
            Assert.Equal("foo", result.Group.Name);
        }

        [Fact]
        public void GroupCreateChildShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""group"":{""name"":""foo""}}");

            var result = context.SlackApi.GroupCreateChild("GROUPID");

            context.VerifyOk(result);
            Assert.Equal("/groups.createChild?channel=GROUPID", context.RequestMade.Resource);
            Assert.Equal("foo", result.Group.Name);
        }

        [Fact]
        public void GroupHistoryShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""messages"":[{""type"":""message"",""text"":""hello""},{""type"":""message"",""subtype"":""me_message"",""text"":""me""}]}");

            var result = context.SlackApi.GroupHistory("foo", messageCount: 44);

            context.VerifyOk(result);
            Assert.Equal("/groups.history?channel=foo&inclusive=0&count=44", context.RequestMade.Resource);
            Assert.IsType<PlainMessage>(result.Messages[0]);
            Assert.Equal(MessageSubType.PlainMessage, result.Messages[0].Subtype);
            Assert.Equal("hello", result.Messages[0].Text);
            Assert.IsType<MeMessage>(result.Messages[1]);
            Assert.Equal(MessageSubType.MeMessage, result.Messages[1].Subtype);
        }

        [Fact]
        public void GroupInfoShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""group"":{""name"":""foo""}}");

            var result = context.SlackApi.GroupInfo("foo");

            context.VerifyOk(result);
            Assert.Equal("/groups.info?channel=foo", context.RequestMade.Resource);
            Assert.Equal("foo", result.Group.Name);
        }

        [Fact]
        public void GroupInviteShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""group"":{""name"":""foo""}}");

            var result = context.SlackApi.GroupInvite("foo", "uid");

            context.VerifyOk(result);
            Assert.Equal("/groups.invite?channel=foo&user=uid", context.RequestMade.Resource);
            Assert.Equal("foo", result.Group.Name);
        }

        [Fact]
        public void GroupKickShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true}");

            var result = context.SlackApi.GroupKick("GROUPID", "UID");

            context.VerifyOk(result);
            Assert.Equal("/groups.kick?channel=GROUPID&user=UID", context.RequestMade.Resource);
        }

        [Fact]
        public void GroupLeaveShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true}");

            var result = context.SlackApi.GroupLeave("CHANID");

            context.VerifyOk(result);
            Assert.Equal("/groups.leave?channel=CHANID", context.RequestMade.Resource);
        }

        [Fact]
        public void GroupListShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""groups"":[{""id"":""GROUPID""}]}");

            var result = context.SlackApi.GroupList();

            context.VerifyOk(result);
            Assert.Equal("/groups.list?exclude_archived=0", context.RequestMade.Resource);
            Assert.Equal("GROUPID", result.Groups[0].Id);
        }

        [Fact]
        public void GroupMarkShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true}");

            var result = context.SlackApi.GroupMark("GROUPID", "1111.2222");

            context.VerifyOk(result);
            Assert.Equal("/groups.mark?channel=GROUPID&ts=1111.2222", context.RequestMade.Resource);
        }

        [Fact]
        public void GroupRenameShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""channel"":{""name"":""foo""}}");

            var result = context.SlackApi.GroupRename("foo", "uid");

            context.VerifyOk(result);
            Assert.Equal("/groups.rename?channel=foo&name=uid", context.RequestMade.Resource);
            Assert.Equal("foo", result.Channel.Name);
        }

        [Fact]
        public void GroupSetPurposeShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""purpose"":""hello""}");

            var result = context.SlackApi.GroupSetPurpose("foo", "hello");

            context.VerifyOk(result);
            Assert.Equal("/groups.setPurpose?channel=foo&purpose=hello", context.RequestMade.Resource);
            Assert.Equal("hello", result.Purpose);
        }

        [Fact]
        public void GroupSetTopicShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""topic"":""hello""}");

            var result = context.SlackApi.GroupSetTopic("foo", "hello");

            context.VerifyOk(result);
            Assert.Equal("/groups.setTopic?channel=foo&topic=hello", context.RequestMade.Resource);
            Assert.Equal("hello", result.Topic);
        }

        [Fact]
        public void GroupUnarchiveShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true}");

            var result = context.SlackApi.GroupUnarchive("foo");

            context.VerifyOk(result);
            Assert.Equal("/groups.unarchive?channel=foo", context.RequestMade.Resource);
        }

        [Fact]
        public void ImCloseShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true}");

            var result = context.SlackApi.ImClose("IMID");

            context.VerifyOk(result);
            Assert.Equal("/im.close?channel=IMID", context.RequestMade.Resource);
        }

        [Fact]
        public void ImHistoryShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""messages"":[{""type"":""message"",""text"":""hello""},{""type"":""message"",""subtype"":""me_message"",""text"":""me""}]}");

            var result = context.SlackApi.ImHistory("foo", messageCount: 44);

            context.VerifyOk(result);
            Assert.Equal("/im.history?channel=foo&inclusive=0&count=44", context.RequestMade.Resource);
            Assert.IsType<PlainMessage>(result.Messages[0]);
            Assert.Equal(MessageSubType.PlainMessage, result.Messages[0].Subtype);
            Assert.Equal("hello", result.Messages[0].Text);
            Assert.IsType<MeMessage>(result.Messages[1]);
            Assert.Equal(MessageSubType.MeMessage, result.Messages[1].Subtype);
        }

        [Fact]
        public void ImListShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""ims"":[{""id"":""IMID""}]}");

            var result = context.SlackApi.ImList();

            context.VerifyOk(result);
            Assert.Equal("/im.list?exclude_archived=0", context.RequestMade.Resource);
            Assert.Equal("IMID", result.Ims[0].Id);
        }

        [Fact]
        public void ImMarkShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true}");

            var result = context.SlackApi.ImMark("IMID", "1111.2222");

            context.VerifyOk(result);
            Assert.Equal("/im.mark?channel=IMID&ts=1111.2222", context.RequestMade.Resource);
        }

        [Fact]
        public void ImOpenShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""channel"":{""id"":""IMID""}}");

            var result = context.SlackApi.ImOpen("UID");

            context.VerifyOk(result);
            Assert.Equal("/im.open?user=UID", context.RequestMade.Resource);
            Assert.Equal("IMID", result.Channel.Id);
        }

        [Fact]
        public void OauthAccessShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""access_token"":""xyz"",""scope"":""read""}");

            var result = context.SlackApi.OauthAccess("CLIENTID", "SECRET", "XXX");

            Assert.NotNull(result);
            Assert.Equal("/oauth.access?client_id=CLIENTID&client_secret=SECRET&code=XXX", context.RequestMade.Resource);
            Assert.Equal("xyz", result.AccessToken);
        }

        [Fact]
        public void StarsListShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""items"":[{""type"":""message"",""message"":{""text"":""hi""}}]}");

            var result = context.SlackApi.StarsList();

            context.VerifyOk(result);
            Assert.Equal("/stars.list?", context.RequestMade.Resource);
            Assert.Equal(1, result.Items.Count);
            Assert.Equal("hi", result.Items[0].Message.Text);
        }

        [Fact]
        public void TeamAccessLogsShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""logins"":[{""username"":""bob""}]}");

            var result = context.SlackApi.TeamAccessLogs();

            context.VerifyOk(result);
            Assert.Equal("/team.accessLogs?", context.RequestMade.Resource);
            Assert.Equal(1, result.Logins.Count);
            Assert.Equal("bob", result.Logins[0].Username);
        }

        [Fact]
        public void TeamInfoShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""team"":{""id"":""TID""}}");

            var result = context.SlackApi.TeamInfo();

            context.VerifyOk(result);
            Assert.Equal("/team.info", context.RequestMade.Resource);
            Assert.Equal("TID", result.Team.Id);
        }

        [Fact]
        public void UsersGetPresenceShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""presence"":""away""}");

            var result = context.SlackApi.UsersGetPresence("UID");

            context.VerifyOk(result);
            Assert.Equal("/users.getPresence?user=UID", context.RequestMade.Resource);
            Assert.Equal("away", result.Presence);
        }

        [Fact]
        public void UsersInfoShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""user"":{""id"":""UID""}}");

            var result = context.SlackApi.UsersInfo("UID");

            context.VerifyOk(result);
            Assert.Equal("/users.info?user=UID", context.RequestMade.Resource);
            Assert.Equal("UID", result.User.Id);
        }

        [Fact]
        public void UsersListShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true,""members"":[{""id"":""UID""}]}");

            var result = context.SlackApi.UsersList();

            context.VerifyOk(result);
            Assert.Equal("/users.list", context.RequestMade.Resource);
            Assert.Equal("UID", result.Members[0].Id);
        }

        [Fact]
        public void UsersSetActiveShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true}");

            var result = context.SlackApi.UsersSetActive();

            context.VerifyOk(result);
            Assert.Equal("/users.setActive", context.RequestMade.Resource);
        }

        [Fact]
        public void UsersSetPresenceShouldReturnResponse()
        {
            var context = SetupTestContext(@"{""ok"":true}");

            var result = context.SlackApi.UsersSetPresence("away");

            context.VerifyOk(result);
            Assert.Equal("/users.setPresence?presence=away", context.RequestMade.Resource);
        }

        internal class TestContext
        {
            internal ISlackApi SlackApi { get; set; }
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

            context.SlackApi = new SlackApi("mockkey")
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
