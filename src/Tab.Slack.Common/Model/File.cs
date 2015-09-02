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
        public string Mimetype { get; set; }
        public string Filetype { get; set; }
        public string PrettyType { get; set; }
        public string Username { get; set; }
        public string Mode { get; set; }
        public bool Editable { get; set; }
        public bool IsExternal { get; set; }
        public bool DisplayAsBot { get; set; }
        public string ExternalType { get; set; }

        public long Size { get; set; }

        public string Url { get; set; }
        public string UrlDownload { get; set; }
        public string UrlPrivate { get; set; }
        public string UrlPrivateDownload { get; set; }
        public string Thumb_64 { get; set; }
        public string Thumb_80 { get; set; }
        public string Thumb_160 { get; set; }
        public string Thumb_360Gif { get; set; }
        public string Thumb_360 { get; set; }
        public int Thumb_360W { get; set; }
        public int Thumb_360H { get; set; }
        public string Thumb_480 { get; set; }
        public int Thumb_480W { get; set; }
        public int Thumb_480H { get; set; }
        public string Thumb_720 { get; set; }
        public int Thumb_720W { get; set; }
        public int Thumb_720H { get; set; }
        public string ImageExifRotation { get; set; }
        public int OriginalW { get; set; }
        public int OriginalH { get; set; }
        public string Permalink { get; set; }
        public string PermalinkPublic { get; set; }
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
        public int CommentsCounts { get; set; }

        // TODO: Ims[] 
    }
}
