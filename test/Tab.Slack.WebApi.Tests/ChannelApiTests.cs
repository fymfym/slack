using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model;
using Tab.Slack.Common.Model.Events;
using Tab.Slack.Common.Model.Requests;
using Tab.Slack.Common.Model.Responses;
using Xunit;

namespace Tab.Slack.WebApi.Tests
{
    public class ChannelApiTests : ApiTestBase
    {
        [Fact]
        public void ChannelArchiveShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<ResponseBase>("/channels.archive?channel=CHANID");

            var subject = new ChannelsApi(requestHandlerMock.Object);
            var result = subject.Archive("CHANID");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void ChannelCreateShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<ChannelResponse>("/channels.create?name=foo");

            var subject = new ChannelsApi(requestHandlerMock.Object);
            var result = subject.Create("foo");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void ChannelJoinShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<ChannelResponse>("/channels.join?name=foo");

            var subject = new ChannelsApi(requestHandlerMock.Object);
            var result = subject.Join("foo");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void ChannelHistoryShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<MessagesResponse>("/channels.history?channel=foo&inclusive=0&count=44");

            var subject = new ChannelsApi(requestHandlerMock.Object);
            var result = subject.History("foo", messageCount: 44);

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void ChannelInfoShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<ChannelResponse>("/channels.info?channel=foo");

            var subject = new ChannelsApi(requestHandlerMock.Object);
            var result = subject.Info("foo");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void ChannelInviteShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<ChannelResponse>("/channels.invite?channel=foo&user=uid");
            
            var subject = new ChannelsApi(requestHandlerMock.Object);
            var result = subject.Invite("foo", "uid");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void ChannelKickShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<ResponseBase>("/channels.kick?channel=CHANID&user=UID");

            var subject = new ChannelsApi(requestHandlerMock.Object);
            var result = subject.Kick("CHANID", "UID");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void ChannelLeaveShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<LeaveChannelResponse>("/channels.leave?channel=CHANID");

            var subject = new ChannelsApi(requestHandlerMock.Object);
            var result = subject.Leave("CHANID");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void ChannelsListShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<ChannelsResponse>("/channels.list?exclude_archived=0");

            var subject = new ChannelsApi(requestHandlerMock.Object);
            var result = subject.List();

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void ChannelMarkShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<ResponseBase>("/channels.mark?channel=CHANID&ts=1111.2222");

            var subject = new ChannelsApi(requestHandlerMock.Object);
            var result = subject.Mark("CHANID", "1111.2222");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void ChannelRenameShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<ChannelResponse>("/channels.rename?channel=foo&name=bar");

            var subject = new ChannelsApi(requestHandlerMock.Object);
            var result = subject.Rename("foo", "bar");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void ChannelSetPurposeShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<PurposeResponse>("/channels.setPurpose?channel=foo&purpose=hello");

            var subject = new ChannelsApi(requestHandlerMock.Object);
            var result = subject.SetPurpose("foo", "hello");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void ChannelSetTopicShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<TopicResponse>("/channels.setTopic?channel=foo&topic=hello");

            var subject = new ChannelsApi(requestHandlerMock.Object);
            var result = subject.SetTopic("foo", "hello");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void ChannelUnarchiveShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<ResponseBase>("/channels.unarchive?channel=foo");

            var subject = new ChannelsApi(requestHandlerMock.Object);
            var result = subject.Unarchive("foo");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }
    }
}
