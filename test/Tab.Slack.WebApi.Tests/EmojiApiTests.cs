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
    public class EmojiApiTests : ApiTestBase
    {
        [Fact]
        public void EmojiListShouldCallCorrectEndpoint()
        {
            var requestHandlerMock = ExecRequestMock<EmojiResponse>("/emoji.list");

            var subject = new EmojiApi(requestHandlerMock.Object);
            var result = subject.List();

            requestHandlerMock.Verify();
            Assert.NotNull(result);
        }
    }
}
