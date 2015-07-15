using DummyTestPlugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Bot.CoreHandlers;
using Xunit;

namespace Tab.Slack.Bot.Tests
{
    public class SlackBotBuilderTests
    {
        [Fact]
        public void InstantiateBasicInstance()
        {
            var slackBot = SlackBot.Build("xxx")
                                   .WithoutCoreHandlers()
                                   .Instantiate();

            Assert.NotNull(slackBot);
            Assert.Null(slackBot.MessageHandlers);
            Assert.Null(slackBot.SlackService);
        }

        [Fact]
        public void InstantiateInstanceWithCoreHandlers()
        {
            var slackBot = SlackBot.Build("xxx")
                                   .Instantiate();

            Assert.NotNull(slackBot);
            Assert.NotNull(slackBot.MessageHandlers);
            Assert.NotNull(slackBot.SlackService);

            Assert.True(slackBot.MessageHandlers.Any(h => h.GetType() == typeof(RtmStartHandler)));
        }

        [Fact]
        public void InstantiateInstanceWithTypePlugin()
        {
            var slackBot = SlackBot.Build("xxx")
                                   .WithoutCoreHandlers()
                                   .WithPlugin<UserPingHandler>()
                                   .Instantiate();

            Assert.NotNull(slackBot);
            Assert.True(slackBot.MessageHandlers.Count() == 1);
            Assert.NotNull(slackBot.SlackService);

            Assert.True(slackBot.MessageHandlers.First().GetType() == typeof(UserPingHandler));
        }

        [Fact]
        public void InstantiateInstanceWithAssemblyPlugin()
        {
            var slackBot = SlackBot.Build("xxx")
                                   .WithoutCoreHandlers()
                                   .WithPluginAssembly(typeof(TestPlugin1).Assembly)
                                   .Instantiate();

            Assert.NotNull(slackBot);
            Assert.True(slackBot.MessageHandlers.Count() == 2);
            Assert.NotNull(slackBot.SlackService);

            Assert.True(slackBot.MessageHandlers.Any(h => h.GetType() == typeof(TestPlugin1)));
            Assert.True(slackBot.MessageHandlers.Any(h => h.GetType() == typeof(TestPlugin2)));
        }

        [Fact]
        public void InstantiateInstanceWithPluginPath()
        {
            var slackBot = SlackBot.Build("xxx")
                                   .WithoutCoreHandlers()
                                   .WithPluginPath("../../../DummyTestPlugins/Dll/")
                                   .Instantiate();

            Assert.NotNull(slackBot);
            Assert.True(slackBot.MessageHandlers.Count() == 2);
            Assert.NotNull(slackBot.SlackService);

            Assert.True(slackBot.MessageHandlers.Any(h => h.GetType() == typeof(TestPlugin1)));
            Assert.True(slackBot.MessageHandlers.Any(h => h.GetType() == typeof(TestPlugin2)));
        }
    }
}
