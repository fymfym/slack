using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tab.Slack.Common.Model;

namespace Tab.Slack.Bot
{
    public interface IBotState
    {
        bool Connected { get; set; }

        string LatestEventTs { get; set; }
        string CacheVersion { get; set; }

        SelfBotData Self { get; set; }

        TeamData Team { get; set; }

        List<BotModel> Bots { get; set; }
        List<User> Users { get; set; }
        List<Channel> Channels { get; set; }
        List<Group> Groups { get; set; }
        List<DirectMessageChannel> Ims { get; set; }
    }
}
