using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tab.Slack.Common.Model.Requests
{
    public class FilesListRequest
    {
        public string User { get; set; }
        public string TsFrom { get; set; }
        public string TsTo { get; set; }
        public string Types { get; set; }
        public int? Count { get; set; }
        public int? Page { get; set; }
    }
}
