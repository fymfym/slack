using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Bot.Integration;
using Tab.Slack.Common.Json;
using Tab.Slack.WebApi;

namespace Tab.Slack.Bot
{
    public class SlackBotBuilder : ISlackBotBuilder
    {
        private ISlackBot slackBot;
        private string apiKey;
        private List<Type> pluginTypes = new List<Type>();
        private List<Assembly> pluginAssemblies = new List<Assembly>();
        private List<string> pluginPaths = new List<string>();
        private bool includeCoreHandlers = true;

        internal SlackBotBuilder(ISlackBot slackBot, string apiKey)
        {
            this.slackBot = slackBot;
            this.apiKey = apiKey;
        }

        public ISlackBot Instantiate()
        {
            var hasPlugins = pluginTypes.Any() || pluginAssemblies.Any() || pluginPaths.Any() || this.includeCoreHandlers;

            if (!hasPlugins)
                return this.slackBot;

            var mefContainer = CreateCompositionContainer();
            
            mefContainer.SatisfyImportsOnce(slackBot);

            // TODO: use SortedList instead?
            if (slackBot.MessageHandlers != null)
                slackBot.MessageHandlers = slackBot.MessageHandlers.OrderByDescending(m => m.Priority).ToArray();

            return slackBot;
        }

        public ISlackBotBuilder WithoutCoreHandlers()
        {
            this.includeCoreHandlers = false;
            return this;
        }

        public ISlackBotBuilder WithPlugin<T>()
        {
            this.pluginTypes.Add(typeof(T));
            return this;
        }

        public ISlackBotBuilder WithPluginAssembly(Assembly pluginAssembly)
        {
            this.pluginAssemblies.Add(pluginAssembly);
            return this;
        }

        public ISlackBotBuilder WithPluginPath(string pluginPath)
        {
            this.pluginPaths.Add(pluginPath);
            return this;
        }

        private CompositionContainer CreateCompositionContainer()
        {
            var aggregateCatalog = new AggregateCatalog();
            var mefContainer = new CompositionContainer(aggregateCatalog);

            if (this.includeCoreHandlers)
                aggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(ISlackBot).Assembly));

            if (this.pluginTypes.Any())
                aggregateCatalog.Catalogs.Add(new TypeCatalog(this.pluginTypes));

            foreach (var pluginDirectoryPath in this.pluginPaths)
            {
                if (!string.IsNullOrWhiteSpace(pluginDirectoryPath))
                    aggregateCatalog.Catalogs.Add(new DirectoryCatalog(pluginDirectoryPath));
            }

            foreach (var assembly in this.pluginAssemblies)
            {
                aggregateCatalog.Catalogs.Add(new AssemblyCatalog(assembly));
            }

            SetupDefaultServices(mefContainer);

            return mefContainer;
        }

        private void SetupDefaultServices(CompositionContainer mefContainer)
        {
            if (mefContainer.GetExportedValueOrDefault<IResponseParser>() == null)
                mefContainer.ComposeExportedValue<IResponseParser>(new ResponseParser());

            if (mefContainer.GetExportedValueOrDefault<IBotServices>() == null)
                mefContainer.ComposeExportedValue<IBotServices>(new SlackSendQueueService());

            if (mefContainer.GetExportedValueOrDefault<IBotState>() == null)
                mefContainer.ComposeExportedValue<IBotState>(new BotState());

            if (mefContainer.GetExportedValueOrDefault<ISlackApi>() == null)
                mefContainer.ComposeExportedValue<ISlackApi>(SlackApi.Create(this.apiKey));

            if (mefContainer.GetExportedValueOrDefault<IBackOffStrategy>() == null)
                mefContainer.ComposeExportedValue<IBackOffStrategy>(new BackOffRetry());

            if (mefContainer.GetExportedValueOrDefault<ILog>() == null)
                mefContainer.ComposeExportedValue<ILog>(LogManager.GetLogger(typeof(ISlackBot)));
        }
    }
}
