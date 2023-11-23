namespace Fanex.OddsFeed.Models
{
    using System;

    [Serializable]
    public class RecentActivityStatistic
    {
        public int Row { get; set; }
        public string Username { get; set; }
        public string Detail { get; set; }
        public DateTime LoggedDate { get; set; }
    }
}