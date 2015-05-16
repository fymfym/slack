using System.Collections.Generic;

namespace Tab.Slack.Common.Model
{
    public class Attachment : FlexibleJsonModel
    {
        public string Fallback { get; set; }
        public string Color { get; set; }
        public string PreText { get; set; }
        

        public string AuthorName { get; set; }
        public string AuthorLink { get; set; }
        public string AuthorIcon { get; set; }

        public string Title { get; set; }
        public string TitleLink { get; set; }

        public string Text { get; set; }

        public List<Field> Fields { get; set; }

        public string ImageUrl { get; set; }
    }
}
