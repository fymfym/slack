using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tab.Slack.Common.Model.Responses
{
    public class SearchResponse : ResponseBase
    {
        public string Query { get; set; }
        public SearchMessagesResponse Messages { get; set; }
        public SearchFilesResponse Files { get; set; }
    }
}
