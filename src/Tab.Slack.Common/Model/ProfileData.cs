namespace Tab.Slack.Common.Model
{
    public class ProfileData : FlexibleJsonModel
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RealName { get; set; }
        public string RealNameNormalized { get; set; }

        public string Email { get; set; }
        public string Skype { get; set; }
        public string Phone { get; set; }

        public string Image_24 { get; set; }
        public string Image_32 { get; set; }
        public string Image_48 { get; set; }
        public string Image_72 { get; set; }
        public string Image_192 { get; set; }
        public string Image_Original { get; set; }
    }
}