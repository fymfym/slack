using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Tab.Slack.Common.Model
{
    public class FlexibleJsonModel
    {
        [JsonExtensionData]
        public JObject UnmatchedAdditionalJsonData { get; set; }
    }
}
