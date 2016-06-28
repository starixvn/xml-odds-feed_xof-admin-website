// --------------------------------------------------------------------------
// <copyright file="DbConnect.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Library
{
    using System;
    using System.Collections;
    using Fanex.Data.DNAide;

    /// <summary>
    /// Class DBConnect.
    /// </summary>
    public static class DbConnect
    {
        public const string Archive = "archive";
        public const string Maintenance = "maintenance";
        public const string Bodb02 = "bodb02";
        public const string Oddsfeed = "oddsfeed";

        public const string BetdaqArchive = "betdaq_archive";
        public const string BetdaqBodb02 = "betdaq_bodb02";

        public static DbResult GetLiveOddsData()
        {
            return DbHelper.ExecuteDbResult(Utilities.RealSp("SP.LiveOdds"));
        }

        public static DbResult GetDeadballOddsData()
        {
            return DbHelper.ExecuteDbResult(Utilities.RealSp("SP.DeadballOdds"));
        }

        public static DbResult GetOutrightOddsData()
        {
            return DbHelper.ExecuteDbResult(Utilities.RealSp("SP.OutrightOdds"));
        }

        public static DbResult GetProfile(int profileId)
        {
            return DbHelper.ExecuteDbResult(Utilities.RealSp("SP.GetProfile"), new Hashtable { { "profileid", profileId } });
        }

        public static DbResult GetProfile(string userName)
        {
            return DbHelper.ExecuteDbResult(Utilities.RealSp("SP.GetProfileByName"), new Hashtable { { "username", userName } });
        }
        
        //// ocs
        public static DbResult GetOcsData(string tableName, string domain, string ip, string username, int? fromid = null)
        {
            var procName = Utilities.RealSp("SP.Ocs_" + tableName);
            var hashtable = new Hashtable { { "domain", domain }, { "ip", ip }, { "username", username } };
            if (fromid != null)
            {
                hashtable.Add("fromid", fromid);
            }

            return DbHelper.ExecuteDbResult(procName, hashtable);
        }

        public static DbResult GetBetdaqScheduleData(string uspName)
        {
            var procName = Utilities.RealSp(uspName);
            var result = DbHelper.ExecuteDbResult(procName);
            return result;
        }

        public static DbResult GetBetRadar()
        {
            return DbHelper.ExecuteDbResult(Utilities.RealSp("SP.BetRadarOddsSel"));
        }

        public static DbResult GetSportByUserName(string userName)
        {
            return DbHelper.ExecuteDbResult(Utilities.RealSp("SP.GetSportByUserName"), new Hashtable { { "username", userName } });      
        }

        public static DbResult GetBetTypeByUserName(string userName)
        {
            return DbHelper.ExecuteDbResult(Utilities.RealSp("SP.GetBetTypeByUserName"), new Hashtable { { "username", userName } });                     
        }

        public static DbResult Odds_CountrySelByUserName(string userName)
        {
            return DbHelper.ExecuteDbResult(Utilities.RealSp("SP.GetCountryByUserName"), new Hashtable { { "username", userName } });
        }

        public static string GetContactEmailByUserName(string userName)
        {
            return Convert.ToString(DbHelper.ExecuteScalar(Utilities.RealSp("SP.GetContactEmailByUserName"), new Hashtable { { "username", userName } }));
        }

        public static DbResult GetCountryList()
        {
            return DbHelper.ExecuteDbResult(Utilities.RealSp("SP.GetCountryList"));
        }
    }
}
