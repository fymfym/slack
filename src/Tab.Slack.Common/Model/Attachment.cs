using System.Collections.Generic;

namespace Tab.Slack.Common.Model
{
    public class Attachment : FlexibleJsonModel
    {
        public string Fallback { get; set; }

        public string Text { get; set; }
        public string PreText { get; set; }

        public string Color { get; set; }
        
        public List<Field> Fields { get; set; }
    }
}
