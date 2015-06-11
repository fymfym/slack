using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tab.Slack.Common.Model.Events;
using Tab.Slack.Common.Model.Events.Messages;

namespace Tab.Slack.Bot.Integration
{
    public static class EventMessageExtensions
    {
        public static bool IsOneOf(this EventMessageBase message, params EventType[] types)
        {
            if (message == null || types == null)
                return false;

            return types.Contains(message.Type);
        }

        public static T CastTo<T>(this EventMessageBase message) where T : EventMessageBase
        {
            var castMessage = message as T;

            if (castMessage == null)
                throw new InvalidCastException($"message is not a valid {typeof(T).Name}");

            return castMessage;
        }

        public static bool IsActivePlainMessage(this EventMessageBase message)
        {
            if (message?.Type != EventType.Message)
                return false;

            var messageBase = CastTo<MessageBase>(message);

            if (messageBase.Subtype != MessageSubType.PlainMessage)
                return false;

            return CastTo<PlainMessage>(message).Historic == false;
        }

        public static bool IsIm(this EventMessageBase message, IBotState botState)
        {
            if (botState == null)
                throw new ArgumentNullException(nameof(botState));

            if (!IsActivePlainMessage(message))
                return false;

            var messageBase = CastTo<MessageBase>(message);

            if (botState.Ims.Any(i => i.Id == messageBase.Channel))
                return true;

            return false;
        }

        public static bool IsToMe(this EventMessageBase message, IBotState botState)
        {
            if (!IsActivePlainMessage(message))
                return false;

            if (IsIm(message, botState))
                return true;

            var messageBase = CastTo<MessageBase>(message);

            return (messageBase.Text ?? "").StartsWith($"<@{botState.Self.Id}>:");
        }

        public static bool MatchesText(this EventMessageBase message, string pattern, RegexOptions options = RegexOptions.None)
        {
            if (!IsActivePlainMessage(message))
                return false;

            var messageBase = CastTo<MessageBase>(message);

            return Regex.IsMatch(messageBase.Text, pattern, options);
        }
    }
}
