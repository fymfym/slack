using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tab.Slack.Common.Model.Responses
{
    public class ChatUpdateResponse : ResponseBase
    {
        public string Ts { get; set; }
        public string Channel { get; set; }
        public string Text { get; set; }
    }
}
