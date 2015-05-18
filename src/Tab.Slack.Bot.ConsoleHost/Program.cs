using System;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Tab.Slack.Bot.ConsoleHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var apiKey = ConfigurationManager.AppSettings["slackbot.apikey"];
            var pluginDir = ConfigurationManager.AppSettings["slackbot.plugindir"];

            using (var slackBot = SlackBot.Create(apiKey, pluginDir))
            {
                slackBot.Start();
            }
        }
    }
}
