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

            var slackBot = SlackBot.Build(apiKey).Instantiate();

            slackBot.Start();

            var stopRequest = false;

            while (stopRequest == false)
            {
                char command = Console.ReadKey(true).KeyChar;

                if (command == 'q')
                {
                    Console.WriteLine("Quitting...");
                    stopRequest = true;
                }

                if (command == 'r')
                {
                    Console.WriteLine("Restarting...");
                    slackBot.Stop();
                    slackBot.Start();
                }

                if (command == 's')
                {
                    Console.WriteLine("Stopping...");
                    slackBot.Stop();
                    stopRequest = true;
                }
            }

            Console.WriteLine("Disposing...");
            slackBot.Dispose();

            Console.WriteLine("Finished.");
        }
    }
}
