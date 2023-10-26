namespace Fanex.OddsFeed.Core.Model
{
    using System;

    [Serializable]
    public class LicenseeRequestStatistic
    {
        public int Row { get; set; }
        public int SystemId { get; set; }
        public string SystemName { get; set; }
        public string Date { get; set; }
        public string Year { get; set; }
        public int TotalRequest { get; set; }
    }
}