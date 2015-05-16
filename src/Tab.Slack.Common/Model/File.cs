using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tab.Slack.Common.Model
{
    public class File : FlexibleJsonModel
    {
        public string Id { get; set; }
        public long Created { get; set; }
        public long Timestamp { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string MimeType { get; set; }
        public string FileType { get; set; }
        public string PrettyType { get; set; }
        public string User { get; set; }
        public string Mode { get; set; }
        public bool Editable { get; set; }
        public bool IsExternal { get; set; }
        public string ExternalType { get; set; }

        public long Size { get; set; }

        public string Url { get; set; }
        public string UrlDownload { get; set; }
        public string UrlPrivate { get; set; }
        public string UrlPrivateDownload { get; set; }
        public string Thumb64 { get; set; }
        public string Thumb80 { get; set; }
        public string Thumb360 { get; set; }
        public string Thumb360Gif { get; set; }
        public int Thumb360W { get; set; }
        public int Thumb360H { get; set; }
        public string Permalink { get; set; }
        public string EditLink { get; set; }
        public string Preview { get; set; }
        public string PreviewHighlight { get; set; }
        public int Lines { get; set; }
        public int LinesMore { get; set; }
        public bool IsPublic { get; set; }
        public bool PublicUrlShared { get; set; }
        public List<string> Channels { get; set; }
        public List<string> Groups { get; set; }
        public ItemComment InitialComment { get; set; }
        public int NumStars { get; set; }
        public bool IsStarred { get; set; }
    }
}
