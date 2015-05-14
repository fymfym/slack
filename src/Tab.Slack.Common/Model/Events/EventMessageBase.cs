using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tab.Slack.Common.Model.Events
{
    public abstract class EventMessageBase :  FlexibleJsonModel
    {
        public EventType Type { get; set; }
    }
}
