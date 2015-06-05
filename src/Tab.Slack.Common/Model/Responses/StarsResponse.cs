using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tab.Slack.Common.Model.Responses
{
    public class StarsResponse : ResponseBase
    {
        public List<StarItemResponse> Items { get; set; }
        public PagingSettings Paging { get; set; }
    }
}
