namespace Fanex.OddsFeed.Common
{
    using System;
    using System.Collections;
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

        public static DbResult GetProfile(int profileId)
        {
            return DbHelper.ExecuteDbResult(SpHelper.RealSp("SP.GetProfile"), new Hashtable { { "profileid", profileId } });
        }

        public static DbResult GetProfile(string userName)
        {
            return DbHelper.ExecuteDbResult(SpHelper.RealSp("SP.GetProfileByName"), new Hashtable { { "username", userName } });
        }

        public static DbResult GetOcsData(string tableName, string domain, string ip, string username, int? fromid = null)
        {
            var procName = SpHelper.RealSp("SP.Ocs_" + tableName);
            var hashtable = new Hashtable { { "domain", domain }, { "ip", ip }, { "username", username } };

            if (fromid != null)
            {
                hashtable.Add("fromid", fromid);
            }

            return DbHelper.ExecuteDbResult(procName, hashtable);
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

        public static DbResult GetSportByUserName(string userName)
        {
            return DbHelper.ExecuteDbResult(SpHelper.RealSp("SP.GetSportByUserName"), new Hashtable { { "username", userName } });
        }

        public static DbResult GetBetTypeByUserName(string userName)
        {
            return DbHelper.ExecuteDbResult(SpHelper.RealSp("SP.GetBetTypeByUserName"), new Hashtable { { "username", userName } });
        }

        public static DbResult Odds_CountrySelByUserName(string userName)
        {
            return DbHelper.ExecuteDbResult(SpHelper.RealSp("SP.GetCountryByUserName"), new Hashtable { { "username", userName } });
        }

        public static string GetContactEmailByUserName(string userName)
        {
            return Convert.ToString(DbHelper.ExecuteScalar(SpHelper.RealSp("SP.GetContactEmailByUserName"), new Hashtable { { "username", userName } }));
        }

        public static DbResult GetCountryList()
        {
            return DbHelper.ExecuteDbResult(SpHelper.RealSp("SP.GetCountryList"));
        }
    }
}
