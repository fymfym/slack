using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model;

namespace Tab.Slack.Bot
{
    // TODO: make thread safe
    [Export(typeof(IBotState))]
    public class BotState : IBotState
    {
        public bool Connected { get; set; }
        public string LatestEventTs { get; set; }
        public string CacheVersion { get; set; }

        public SelfBotData Self { get; set; }

        public TeamData Team { get; set; }

        public List<BotModel> Bots { get; set; }
        public List<User> Users { get; set; }
        public List<Channel> Channels { get; set; }
        public List<Group> Groups { get; set; }
        public List<DirectMessageChannel> Ims { get; set; }
    }
}
