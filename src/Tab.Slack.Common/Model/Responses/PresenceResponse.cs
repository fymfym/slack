using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tab.Slack.Common.Model.Responses
{
    public class PresenceResponse : ResponseBase
    {
        public string Presence { get; set; }
        public bool Online { get; set; }
        public bool AutoAway { get; set; }
        public bool ManualAway { get; set; }
        public int ConnectionCount { get; set; }
        public int LastActivity { get; set; }
    }
}
