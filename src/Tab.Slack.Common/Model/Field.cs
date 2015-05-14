using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tab.Slack.Common.Model
{
    public class Field : FlexibleJsonModel
    {
        public string Title { get; set; }
        public string Value { get; set; }
        public bool Short { get; set; }
    }
}
