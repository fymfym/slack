using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tab.Slack.Common.Model
{
    public class ItemComment : FlexibleJsonModel
    {
        public string Id { get; set; }
        public string Timestamp { get; set; }
        public string User { get; set; }
        public string Comment { get; set; }
    }
}
