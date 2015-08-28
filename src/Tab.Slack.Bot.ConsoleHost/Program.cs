using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;

namespace Tab.Slack.Bot.ConsoleHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var apiKey = ConfigurationManager.AppSettings["slackbot.apikey"];

            using (var slackBot = SlackBot.Create(apiKey).Instantiate())
            {
                slackBot.Start();

                while (true)
                {
                    char command = Console.ReadKey(true).KeyChar;

                    if (command == 'q')
                    {
                        Console.WriteLine("Quitting...");
                        slackBot.Stop();
                        return;
                    }

                    if (command == 'r')
                    {
                        Console.WriteLine("Restarting...");
                        slackBot.Stop();
                        slackBot.Start();
                    }
                }
            }
        }
    }
}
