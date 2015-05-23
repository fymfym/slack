using System.Collections.Generic;

namespace Tab.Slack.Common.Model.Responses
{
    public class SearchFilesResponse : FlexibleJsonModel
    {
        public List<File> Matches { get; set; }
        public PagingSettings Paging { get; set; }
    }
}