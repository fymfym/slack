using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tab.Slack.Common.Model.Events.Messages
{
    public enum MessageSubType
    {
        PlainMessage,
        BotMessage,
        MeMessage,
        MessageChanged,
        MessageDeleted,
        ChannelJoin,
        ChannelLeave,
        ChannelTopic,
        ChannelPurpose,
        ChannelName,
        ChannelArchive,
        ChannelUnarchive,
        GroupJoin,
        GroupLeave,
        GroupTopic,
        GroupPurpose,
        GroupName,
        GroupArchive,
        GroupUnarchive,
        FileShare,
        FileComment,
        FileMention,
        PinnedItem,
        UnpinnedItem
    }
}
