using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tab.Slack.Common.Model.Responses
{
    public class FileResponse : ResponseBase
    {
        public File File { get; set; }
        public List<ItemComment> Comments { get; set; }
        public PagingSettings Paging { get; set; }
    }
}
