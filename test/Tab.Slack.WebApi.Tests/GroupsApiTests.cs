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
    public class GroupsApiTests : ApiTestBase
    {
        [Fact]
        public void GroupArchiveShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<ResponseBase>("/groups.archive?channel=GROUPID");

            var subject = new GroupsApi(requestHandlerMock.Object);
            var result = subject.Archive("GROUPID");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void GroupCloseShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<CloseResponse>("/groups.close?channel=GROUPID");

            var subject = new GroupsApi(requestHandlerMock.Object);
            var result = subject.Close("GROUPID");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void GroupCreateShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<GroupResponse>("/groups.create?name=foo");

            var subject = new GroupsApi(requestHandlerMock.Object);
            var result = subject.Create("foo");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void GroupCreateChildShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<GroupResponse>("/groups.createChild?channel=GROUPID");

            var subject = new GroupsApi(requestHandlerMock.Object);
            var result = subject.CreateChild("GROUPID");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void GroupHistoryShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<MessagesResponse>("/groups.history?channel=foo&inclusive=0&count=44");

            var subject = new GroupsApi(requestHandlerMock.Object);
            var result = subject.History("foo", messageCount: 44);

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void GroupInfoShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<GroupResponse>("/groups.info?channel=foo");

            var subject = new GroupsApi(requestHandlerMock.Object);
            var result = subject.Info("foo");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void GroupInviteShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<GroupResponse>("/groups.invite?channel=foo&user=uid");

            var subject = new GroupsApi(requestHandlerMock.Object);
            var result = subject.Invite("foo", "uid");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void GroupKickShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<ResponseBase>("/groups.kick?channel=GROUPID&user=UID");

            var subject = new GroupsApi(requestHandlerMock.Object);
            var result = subject.Kick("GROUPID", "UID");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void GroupLeaveShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<ResponseBase>("/groups.leave?channel=CHANID");

            var subject = new GroupsApi(requestHandlerMock.Object);
            var result = subject.Leave("CHANID");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void GroupListShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<GroupsResponse>("/groups.list?exclude_archived=0");

            var subject = new GroupsApi(requestHandlerMock.Object);
            var result = subject.List();

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void GroupMarkShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<ResponseBase>("/groups.mark?channel=GROUPID&ts=1111.2222");

            var subject = new GroupsApi(requestHandlerMock.Object);
            var result = subject.Mark("GROUPID", "1111.2222");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void GroupRenameShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<ChannelResponse>("/groups.rename?channel=foo&name=uid");

            var subject = new GroupsApi(requestHandlerMock.Object);
            var result = subject.Rename("foo", "uid");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void GroupSetPurposeShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<PurposeResponse>("/groups.setPurpose?channel=foo&purpose=hello");

            var subject = new GroupsApi(requestHandlerMock.Object);
            var result = subject.SetPurpose("foo", "hello");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void GroupSetTopicShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<TopicResponse>("/groups.setTopic?channel=foo&topic=hello");

            var subject = new GroupsApi(requestHandlerMock.Object);
            var result = subject.SetTopic("foo", "hello");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void GroupUnarchiveShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<ResponseBase>("/groups.unarchive?channel=foo");

            var subject = new GroupsApi(requestHandlerMock.Object);
            var result = subject.Unarchive("foo");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }
    }
}
