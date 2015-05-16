using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tab.Slack.Common.Model.Responses
{
    public class ChannelsResponse : ResponseBase
    {
        public List<Channel> Channels { get; set; }
    }
}
