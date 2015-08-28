using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model.Responses;
using Xunit;

namespace Tab.Slack.WebApi.Tests
{
    public class UsersApiTests : ApiTestBase
    {
        [Fact]
        public void UsersGetPresenceShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<PresenceResponse>("/users.getPresence?user=UID");

            var subject = new UsersApi(requestHandlerMock.Object);
            var result = subject.GetPresence("UID");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void UsersInfoShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<UserResponse>("/users.info?user=UID");

            var subject = new UsersApi(requestHandlerMock.Object);
            var result = subject.Info("UID");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void UsersListShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = ExecRequestMock<UsersResponse>("/users.list");

            var subject = new UsersApi(requestHandlerMock.Object);
            var result = subject.List();

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void UsersSetActiveShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = ExecRequestMock<ResponseBase>("/users.setActive");

            var subject = new UsersApi(requestHandlerMock.Object);
            var result = subject.SetActive();

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }

        [Fact]
        public void UsersSetPresenceShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = PathAndExecRequestMock<ResponseBase>("/users.setPresence?presence=away");

            var subject = new UsersApi(requestHandlerMock.Object);
            var result = subject.SetPresence("away");

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }
    }
}
