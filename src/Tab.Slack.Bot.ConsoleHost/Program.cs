using SimpleInjector;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common;
using Tab.Slack.Bot.Integration;
using Tab.Slack.Bot.Handlers;
using Tab.Slack.Common.Json;
using Tab.Slack.Rest;

namespace Tab.Slack.Bot.ConsoleHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var mefContainer = BootstrapMefContainer();
            var iocContainer = BootstrapIocContainer(mefContainer);

            // TODO: implement connection retries and error handling
            using (var slackClient = iocContainer.GetInstance<IBotClient>())
            {
                slackClient.Start();
            }
        }

        private static CompositionContainer BootstrapMefContainer()
        {
            var pluginPath = ConfigurationManager.AppSettings["slackbot.plugindir"];

            var slackCatalog = new AssemblyCatalog(typeof(IBotClient).Assembly);
            var coreHandlerCatalog = new AssemblyCatalog(typeof(RtmStartHandler).Assembly);

            var aggregateCatalog = new AggregateCatalog(slackCatalog, coreHandlerCatalog);

            if (!string.IsNullOrWhiteSpace(pluginPath))
                aggregateCatalog.Catalogs.Add(new DirectoryCatalog(pluginPath));
            
            return new CompositionContainer(aggregateCatalog);
        }

        private static Container BootstrapIocContainer(CompositionContainer mefContainer)
        {
            var iocContainer = new Container();

            iocContainer.Register<IResponseParser, ResponseParser>();
            iocContainer.Register<IBotClient, BotClient>(Lifestyle.Transient);
            iocContainer.Register<IBotState>(() => mefContainer.GetExportedValue<IBotState>());
            iocContainer.Register<IBotServices>(() => mefContainer.GetExportedValue<IBotServices>());
            iocContainer.Register<IEnumerable<IMessageHandler>>(() => mefContainer.GetExportedValues<IMessageHandler>());

            var config = EstablishConfig();
            iocContainer.Register<Config>(() => config);

            var slackClient = new SlackClient(config.ApiKey);
            iocContainer.Register<ISlackClient>(() => slackClient);

            return iocContainer;
        }

        private static Config EstablishConfig()
        {
            var apiKey = ConfigurationManager.AppSettings["slackbot.apikey"];

            if (string.IsNullOrWhiteSpace(apiKey))
                throw new Exception("Please update the App.config with your Slack bot API key");

            return new Config()
            {
                ApiKey = apiKey
            };
        }
    }
}
