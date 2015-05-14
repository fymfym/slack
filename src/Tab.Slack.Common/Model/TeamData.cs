using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Tab.Slack.Common.Model
{
    public class TeamData : FlexibleJsonModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string EmailDomain { get; set; }
        public string Domain { get; set; }
        public int MsgEditWindowMins { get; set; }
        public bool OverStorageLimit { get; set; }

        /// <summary>
        /// Team-specific preferences.
        /// </summary>
        public JObject Prefs { get; set; }
    }
}