using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tab.Slack.Common.Model.Responses
{
    public class ImOpenResponse : ResponseBase
    {
        public DirectMessageChannel Channel { get; set; }
        public bool NoOp { get; set; }
        public bool AlreadyOpen { get; set; }
    }
}
