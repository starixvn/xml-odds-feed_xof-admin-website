namespace Fanex.OddsFeed.Core.Model.Report
{
    public class AccountStatistic
    {
        public int TotalAff { get; set; }
        public int TotalSubAcc { get; set; }
        public int TotalTodayRequest { get; set; }
        public int TotalMonthRequest { get; set; }
        public string LastRequestUsername { get; set; }
        public string LastRequestTime { get; set; }
        public string LastRequestIP { get; set; }
    }
}