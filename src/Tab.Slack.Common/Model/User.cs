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
        public string RealName { get; set; }
        public bool Deleted { get; set; }
        public string Color { get; set; }
        public ProfileData Profile { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsOwner { get; set; }
        public bool IsPrimaryOwner { get; set; }
        public bool IsRestricted { get; set; }
        public bool IsUltraRestricted { get; set; }
        public bool IsBot { get; set; }
        public bool HasFiles { get; set; }
        public string Presence { get; set; }
        public string Status { get; set; }
        public string Tz { get; set; }
        public string TzLabel { get; set; }
        public int TzOffset { get; set; }
    }
}
