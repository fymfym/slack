using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tab.Slack.Common.Model.Responses
{
    public class ReactionListResponse : ResponseBase
    {
        public List<ReactionItem> Items { get; set; }
        public PagingSettings Paging { get; set; }
    }
}
