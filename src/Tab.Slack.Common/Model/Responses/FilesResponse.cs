using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tab.Slack.Common.Model.Responses
{
    public class FilesResponse : ResponseBase
    {
        public List<File> Files { get; set; }
        public PagingSettings Paging { get; set; }
    }
}
