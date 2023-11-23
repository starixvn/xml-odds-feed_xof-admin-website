namespace Fanex.OddsFeed.Models
{
    using System;

    [Serializable]
    public class UserOnlineStatistic
    {
        public int Row { get; set; }
        public string Username { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime ReloadTime { get; set; }
        public string IP { get; set; }
        public string RoleName { get; set; }
    }
}