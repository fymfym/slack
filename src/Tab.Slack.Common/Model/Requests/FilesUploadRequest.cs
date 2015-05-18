using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tab.Slack.Common.Model.Requests
{
    public class FileUploadRequest
    {
        public byte[] FileData { get; set; }
        public string Filetype { get; set; }
        public string Filename { get; set; }
        public string Title { get; set; }
        public string InitialComment { get; set; }
        public string Channels { get; set; }
    }
}
