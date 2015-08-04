using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tab.Slack.Common.Model.Requests;
using Tab.Slack.Common.Model.Responses;

namespace Tab.Slack.WebApi
{
    public interface IChatApi
    {
        ChatDeleteResponse Delete(string channelId, string timestamp);
        MessageResponse PostMessage(PostMessageRequest request);
        ChatUpdateResponse Update(string channelId, string timestamp, string updatedText);
    }
}
