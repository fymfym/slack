namespace Tab.Slack.Common.Model.Responses
{
    public class SearchMessageMatch : FlexibleJsonModel
    {
        public string Type { get; set; }
        public SearchChannel Channel { get; set; }
        public string User { get; set; }
        public string Username { get; set; }
        public string Ts { get; set; }
        public string Text { get; set; }
        public string Permalink { get; set; }
        public SearchMessageMatch Previous_2 { get; set; }
        public SearchMessageMatch Previous { get; set; }
        public SearchMessageMatch Next { get; set; }
        public SearchMessageMatch Next_2 { get; set; }
    }
}