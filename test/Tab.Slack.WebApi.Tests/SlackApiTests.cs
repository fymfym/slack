using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tab.Slack.WebApi.Tests
{
    public class SlackApiTests
    {
        public void CreatesWithDefaultRequestHandler()
        {
            var subject = SlackApi.Create("xxx");

            Assert.NotNull(subject);
            Assert.NotNull(subject.Api);
            Assert.NotNull(subject.Auth);
            Assert.NotNull(subject.Channels);
            Assert.NotNull(subject.Chat);
            Assert.NotNull(subject.Emoji);
            Assert.NotNull(subject.Files);
            Assert.NotNull(subject.Groups);
            Assert.NotNull(subject.Im);
            Assert.NotNull(subject.Oauth);
            Assert.NotNull(subject.Reactions);
            Assert.NotNull(subject.Rtm);
            Assert.NotNull(subject.Search);
            Assert.NotNull(subject.Stars);
            Assert.NotNull(subject.Team);
            Assert.NotNull(subject.Users);
        }

        public void CreatesWithProvidedRequestHandler()
        {
            var requestHandler = new Mock<IRequestHandler>();
            var subject = SlackApi.Create(requestHandler.Object);

            Assert.NotNull(subject);
            Assert.NotNull(subject.Api);
            Assert.NotNull(subject.Auth);
            Assert.NotNull(subject.Channels);
            Assert.NotNull(subject.Chat);
            Assert.NotNull(subject.Emoji);
            Assert.NotNull(subject.Files);
            Assert.NotNull(subject.Groups);
            Assert.NotNull(subject.Im);
            Assert.NotNull(subject.Oauth);
            Assert.NotNull(subject.Reactions);
            Assert.NotNull(subject.Rtm);
            Assert.NotNull(subject.Search);
            Assert.NotNull(subject.Stars);
            Assert.NotNull(subject.Team);
            Assert.NotNull(subject.Users);
        }
    }
}
