namespace Fanex.OddsFeed.Common
{
    using Fanex.Data.DNAide;
    using Helpers;

    public static class DbAccess
    {
        public const string Archive = "archive";
        public const string Maintenance = "maintenance";
        public const string Bodb02 = "bodb02";
        public const string Oddsfeed = "oddsfeed";

        public const string BetdaqArchive = "betdaq_archive";
        public const string BetdaqBodb02 = "betdaq_bodb02";

        public static DbResult GetLiveOddsData()
        {
            return DbHelper.ExecuteDbResult(SpHelper.RealSp("SP.LiveOdds"));
        }

        public static DbResult GetDeadballOddsData()
        {
            return DbHelper.ExecuteDbResult(SpHelper.RealSp("SP.DeadballOdds"));
        }

        public static DbResult GetOutrightOddsData()
        {
            return DbHelper.ExecuteDbResult(SpHelper.RealSp("SP.OutrightOdds"));
        }

        public static DbResult GetBetdaqScheduleData(string uspName)
        {
            var procName = SpHelper.RealSp(uspName);
            var result = DbHelper.ExecuteDbResult(procName);
            return result;
        }

        public static DbResult GetBetRadar()
        {
            return DbHelper.ExecuteDbResult(SpHelper.RealSp("SP.BetRadarOddsSel"));
        }

        public static DbResult GetCountryList()
        {
            return DbHelper.ExecuteDbResult(SpHelper.RealSp("SP.GetCountryList"));
        }
    }
}
