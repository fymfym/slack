using System.Collections.Generic;
using Tab.Slack.Common.Model.Events.Messages;

namespace Tab.Slack.Common.Model.Responses
{
    public class SearchMessagesResponse : FlexibleJsonModel
    {
        public List<SearchMessageMatch> Matches { get; set; }
        public PagingSettings Paging { get; set; }
    }
}