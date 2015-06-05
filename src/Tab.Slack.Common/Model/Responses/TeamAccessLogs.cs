using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tab.Slack.Common.Model.Responses
{
    public class TeamAccessLogs : ResponseBase
    {
        public List<TeamAccessLog> Logins { get; set; }
        public PagingSettings Paging { get; set; }
    }
}
