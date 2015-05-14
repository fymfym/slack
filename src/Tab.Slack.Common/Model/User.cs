namespace Tab.Slack.Common.Model
{
    /// <summary>
    /// Contains information about a Slack team member.
    /// More information can be found at https://api.slack.com/types/user.
    /// </summary>
    public class User : FlexibleJsonModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Deleted { get; set; }
        public string Color { get; set; }
        public ProfileData Profile { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsOwner { get; set; }
        public bool IsPrimaryOwner { get; set; }
        public bool IsRestricted { get; set; }
        public bool IsUltraRestricted { get; set; }
        public bool HasFiles { get; set; }
        public string Presence { get; set; }
    }
}
