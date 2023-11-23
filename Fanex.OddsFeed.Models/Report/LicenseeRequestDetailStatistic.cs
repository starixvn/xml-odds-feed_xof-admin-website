namespace Fanex.OddsFeed.Models
{
    using System;

    [Serializable]
    public class LicenseeRequestDetailStatistic
    {
        public int Row { get; set; }
        public string Partner { get; set; }
        public string Domain { get; set; }
        public string IP { get; set; }
        public string Des { get; set; }
        public string Date { get; set; }
        public string LastCallTime { get; set; }
        public int TotalRequest { get; set; }
    }
}