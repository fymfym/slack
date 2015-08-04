//using Moq;
//using RestSharp;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Tab.Slack.Common.Model;
//using Tab.Slack.Common.Model.Events;
//using Tab.Slack.Common.Model.Events.Messages;
//using Tab.Slack.Common.Model.Requests;
//using Tab.Slack.Common.Model.Responses;
//using Xunit;

//namespace Tab.Slack.WebApi.Tests
//{
//    // TODO: refactor this mess
//    public class SlackApiTests
//    {


//        [Fact]
//        public void ImCloseShouldCallCorrectEndpoint()
//        {
//            var context = SetupTestContext(@"{""ok"":true}");

//            var result = context.SlackApi.ImClose("IMID");

//            context.VerifyOk(result);
//            Assert.Equal("/im.close?channel=IMID", context.RequestMade.Resource);
//        }

//        [Fact]
//        public void ImHistoryShouldCallCorrectEndpoint()
//        {
//            var context = SetupTestContext(@"{""ok"":true,""messages"":[{""type"":""message"",""text"":""hello""},{""type"":""message"",""subtype"":""me_message"",""text"":""me""}]}");

//            var result = context.SlackApi.ImHistory("foo", messageCount: 44);

//            context.VerifyOk(result);
//            Assert.Equal("/im.history?channel=foo&inclusive=0&count=44", context.RequestMade.Resource);
//            Assert.IsType<PlainMessage>(result.Messages[0]);
//            Assert.Equal(MessageSubType.PlainMessage, result.Messages[0].Subtype);
//            Assert.Equal("hello", result.Messages[0].Text);
//            Assert.IsType<MeMessage>(result.Messages[1]);
//            Assert.Equal(MessageSubType.MeMessage, result.Messages[1].Subtype);
//        }

//        [Fact]
//        public void ImListShouldCallCorrectEndpoint()
//        {
//            var context = SetupTestContext(@"{""ok"":true,""ims"":[{""id"":""IMID""}]}");

//            var result = context.SlackApi.ImList();

//            context.VerifyOk(result);
//            Assert.Equal("/im.list?exclude_archived=0", context.RequestMade.Resource);
//            Assert.Equal("IMID", result.Ims[0].Id);
//        }

//        [Fact]
//        public void ImMarkShouldCallCorrectEndpoint()
//        {
//            var context = SetupTestContext(@"{""ok"":true}");

//            var result = context.SlackApi.ImMark("IMID", "1111.2222");

//            context.VerifyOk(result);
//            Assert.Equal("/im.mark?channel=IMID&ts=1111.2222", context.RequestMade.Resource);
//        }

//        [Fact]
//        public void ImOpenShouldCallCorrectEndpoint()
//        {
//            var context = SetupTestContext(@"{""ok"":true,""channel"":{""id"":""IMID""}}");

//            var result = context.SlackApi.ImOpen("UID");

//            context.VerifyOk(result);
//            Assert.Equal("/im.open?user=UID", context.RequestMade.Resource);
//            Assert.Equal("IMID", result.Channel.Id);
//        }

//        [Fact]
//        public void OauthAccessShouldCallCorrectEndpoint()
//        {
//            var context = SetupTestContext(@"{""access_token"":""xyz"",""scope"":""read""}");

//            var result = context.SlackApi.OauthAccess("CLIENTID", "SECRET", "XXX");

//            Assert.NotNull(result);
//            Assert.Equal("/oauth.access?client_id=CLIENTID&client_secret=SECRET&code=XXX", context.RequestMade.Resource);
//            Assert.Equal("xyz", result.AccessToken);
//        }

//        [Fact]
//        public void ReactionAddShouldCallCorrectEndpoint()
//        {
//            var context = SetupTestContext(@"{""ok"":true}");

//            var result = context.SlackApi.ReactionAdd("ok", channelId: "CHANID", ts: "123.123");

//            context.VerifyOk(result);
//            Assert.Equal("/reactions.add?name=ok&channel=CHANID&timestamp=123.123", context.RequestMade.Resource);
//        }

//        [Fact]
//        public void ReactionGetShouldCallCorrectEndpoint()
//        {
//            var context = SetupTestContext(@"{""ok"":true,""type"":""message"",""channel"":""CHANID"",""message"":{""reactions"":[{""name"":""ok"",""count"":1,""users"":[""U1""]}]}}");

//            var result = context.SlackApi.ReactionGet(channelId: "CHANID", ts: "123.123");

//            context.VerifyOk(result);
//            Assert.Equal("/reactions.get?channel=CHANID&timestamp=123.123", context.RequestMade.Resource);
//            Assert.Equal("CHANID", result.Channel);
//            Assert.Equal("ok", result.Message.Reactions[0].Name);
//        }

//        [Fact]
//        public void ReactionListShouldCallCorrectEndpoint()
//        {
//            var context = SetupTestContext(@"{""ok"":true,""items"":[{""type"":""file"",""reactions"":[{""name"":""ok""}]}],""paging"":{""count"":1}}");

//            var result = context.SlackApi.ReactionList(userId: "USERID");

//            context.VerifyOk(result);
//            Assert.Equal("/reactions.list?user=USERID", context.RequestMade.Resource);
//            Assert.Equal("ok", result.Items[0].Reactions[0].Name);
//        }

//        [Fact]
//        public void ReactionRemoveShouldCallCorrectEndpoint()
//        {
//            var context = SetupTestContext(@"{""ok"":true}");

//            var result = context.SlackApi.ReactionRemove("ok", channelId: "CHANID", ts: "123.123");

//            context.VerifyOk(result);
//            Assert.Equal("/reactions.remove?name=ok&channel=CHANID&timestamp=123.123", context.RequestMade.Resource);
//        }

//        [Fact]
//        public void StarsListShouldCallCorrectEndpoint()
//        {
//            var context = SetupTestContext(@"{""ok"":true,""items"":[{""type"":""message"",""message"":{""text"":""hi""}}]}");

//            var result = context.SlackApi.StarsList();

//            context.VerifyOk(result);
//            Assert.Equal("/stars.list?", context.RequestMade.Resource);
//            Assert.Equal(1, result.Items.Count);
//            Assert.Equal("hi", result.Items[0].Message.Text);
//        }

//        [Fact]
//        public void TeamAccessLogsShouldCallCorrectEndpoint()
//        {
//            var context = SetupTestContext(@"{""ok"":true,""logins"":[{""username"":""bob""}]}");

//            var result = context.SlackApi.TeamAccessLogs();

//            context.VerifyOk(result);
//            Assert.Equal("/team.accessLogs?", context.RequestMade.Resource);
//            Assert.Equal(1, result.Logins.Count);
//            Assert.Equal("bob", result.Logins[0].Username);
//        }

//        [Fact]
//        public void TeamInfoShouldCallCorrectEndpoint()
//        {
//            var context = SetupTestContext(@"{""ok"":true,""team"":{""id"":""TID""}}");

//            var result = context.SlackApi.TeamInfo();

//            context.VerifyOk(result);
//            Assert.Equal("/team.info", context.RequestMade.Resource);
//            Assert.Equal("TID", result.Team.Id);
//        }

//        [Fact]
//        public void UsersGetPresenceShouldCallCorrectEndpoint()
//        {
//            var context = SetupTestContext(@"{""ok"":true,""presence"":""away""}");

//            var result = context.SlackApi.UsersGetPresence("UID");

//            context.VerifyOk(result);
//            Assert.Equal("/users.getPresence?user=UID", context.RequestMade.Resource);
//            Assert.Equal("away", result.Presence);
//        }

//        [Fact]
//        public void UsersInfoShouldCallCorrectEndpoint()
//        {
//            var context = SetupTestContext(@"{""ok"":true,""user"":{""id"":""UID""}}");

//            var result = context.SlackApi.UsersInfo("UID");

//            context.VerifyOk(result);
//            Assert.Equal("/users.info?user=UID", context.RequestMade.Resource);
//            Assert.Equal("UID", result.User.Id);
//        }

//        [Fact]
//        public void UsersListShouldCallCorrectEndpoint()
//        {
//            var context = SetupTestContext(@"{""ok"":true,""members"":[{""id"":""UID""}]}");

//            var result = context.SlackApi.UsersList();

//            context.VerifyOk(result);
//            Assert.Equal("/users.list", context.RequestMade.Resource);
//            Assert.Equal("UID", result.Members[0].Id);
//        }

//        [Fact]
//        public void UsersSetActiveShouldCallCorrectEndpoint()
//        {
//            var context = SetupTestContext(@"{""ok"":true}");

//            var result = context.SlackApi.UsersSetActive();

//            context.VerifyOk(result);
//            Assert.Equal("/users.setActive", context.RequestMade.Resource);
//        }

//        [Fact]
//        public void UsersSetPresenceShouldCallCorrectEndpoint()
//        {
//            var context = SetupTestContext(@"{""ok"":true}");

//            var result = context.SlackApi.UsersSetPresence("away");

//            context.VerifyOk(result);
//            Assert.Equal("/users.setPresence?presence=away", context.RequestMade.Resource);
//        }




//    }
//}
