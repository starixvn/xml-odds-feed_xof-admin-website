namespace Fanex.OddsFeed.Common
{
    using Data;
    using Helpers;
    using System.Data;

    public static class DbAccess
    {
        public static DataSet GetLiveOddsData()
        {
            using (var db = new ObjectDb(SpHelper.RealSp("SP.LiveOdds")))
            {
                return db.ExecuteDataSet();
            }
        }

        public static DataSet GetDeadballOddsData()
        {
            using (var db = new ObjectDb(SpHelper.RealSp("SP.DeadballOdds")))
            {
                return db.ExecuteDataSet();
            }
        }

        public static DataSet GetOutrightOddsData()
        {
            using (var db = new ObjectDb(SpHelper.RealSp("SP.OutrightOdds")))
            {
                return db.ExecuteDataSet();
            }
        }

        public static DataSet GetBetdaqScheduleData(string uspName)
        {
            using (var db = new ObjectDb(SpHelper.RealSp(uspName)))
            {
                return db.ExecuteDataSet();
            }
        }

        public static DataSet GetBetRadar()
        {
            using (var db = new ObjectDb(SpHelper.RealSp("SP.BetRadarOddsSel")))
            {
                return db.ExecuteDataSet();
            }
        }

        public static DataSet GetCountryList()
        {
            using (var db = new ObjectDb(SpHelper.RealSp("SP.GetCountryList")))
            {
                return db.ExecuteDataSet();
            }
        }
    }
}