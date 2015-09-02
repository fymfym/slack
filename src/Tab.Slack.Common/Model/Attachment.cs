using System.Collections.Generic;

namespace Tab.Slack.Common.Model
{
    public class Attachment : FlexibleJsonModel
    {
        public string Id { get; set; }
        public string Ts { get; set; }

        public string Fallback { get; set; }
        public string Color { get; set; }
        public string Pretext { get; set; }
        public string Footer { get; set; }


        public string AuthorName { get; set; }
        public string AuthorSubname { get; set; }
        public string AuthorLink { get; set; }
        public string AuthorIcon { get; set; }

        public string ServiceName { get; set; }
        public string ServiceUrl { get; set; }
        public string FromUrl { get; set; }

        public string Title { get; set; }
        public string TitleLink { get; set; }

        public string Text { get; set; }

        public List<Field> Fields { get; set; }

        public string ImageUrl { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public long ImageBytes { get; set; }

        // TODO: check if we can replace with enum instead of string
        public List<string> MrkdwnIn { get; set; }
    }
}
