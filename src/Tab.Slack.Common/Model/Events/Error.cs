using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tab.Slack.Common.Model.Events
{
    public class Error : EventMessageBase
    {
        public string Msg { get; set; }
        public string Code { get; set; }
    }
}
