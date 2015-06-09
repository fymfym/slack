using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Bot.Integration;
using Tab.Slack.Common.Json;
using Tab.Slack.WebApi;

namespace Tab.Slack.Bot
{
    internal class Bootstrap
    {
        internal static ISlackBot BuildSlackBot(ISlackBot slackBot, string apiKey, string pluginDirectoryPath, bool includeCoreHandlers = true)
        {
            var aggregateCatalog = new AggregateCatalog();

            if (includeCoreHandlers)
                aggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(ISlackBot).Assembly));

            if (!string.IsNullOrWhiteSpace(pluginDirectoryPath))
                aggregateCatalog.Catalogs.Add(new DirectoryCatalog(pluginDirectoryPath));

            var mefContainer = new CompositionContainer(aggregateCatalog);

            // TODO: use SortedList instead?
            slackBot.MessageHandlers = mefContainer.GetExportedValues<IMessageHandler>();

            if (slackBot.MessageHandlers != null)
                slackBot.MessageHandlers = slackBot.MessageHandlers.OrderByDescending(m => m.Priority).ToArray();

            slackBot.ResponseParser = mefContainer.GetExportedValueOrDefault<IResponseParser>() ?? new ResponseParser();
            slackBot.SlackService = mefContainer.GetExportedValueOrDefault<IBotServices>() ?? new SlackSendQueueService();
            slackBot.SlackState = mefContainer.GetExportedValueOrDefault<IBotState>() ?? new BotState();
            slackBot.SlackApi = mefContainer.GetExportedValueOrDefault<ISlackApi>() ?? new SlackApi(apiKey);

            return slackBot;
        }
    }
}
