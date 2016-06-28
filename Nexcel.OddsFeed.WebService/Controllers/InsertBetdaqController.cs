// --------------------------------------------------------------------------
// <copyright file="InsertBetdaqController.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>
namespace Nexcel.OddsFeed.WebService.Controllers
{
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Data;
    using System.Web.Mvc;
    using Fanex.Data.DNAide;
    using Nexcel.OddsFeed.Library;
    using Nexcel.OddsFeed.WebService.Functions;

    /// <summary>
    /// Class InsertBETDAQController.
    /// </summary>    
    public class InsertBetdaqController : Controller
    {
        /// <summary>
        /// Indexes the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>System String.</returns>
        public string Index(string username)
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsDisableUMTimeFlag"]) == false)
            {
                if (username.ToLower() != "betdaq")
                {
                    return "You are not authorized to take this action.";
                }

                // check UM Time
                string startTime = string.Empty;
                string endTime = string.Empty;

                if (Utilities.GetDownTime(out startTime, out endTime))
                {
                    return "Failed - Bad oddsId";
                }
            }

            var domain = Helper.DomainName(Request.ServerVariables);                         
            
            var betdaqid = Request["RequestId"];
            var oddsid   = Request["OddsId"];
            var hdp1     = Request["Handicap1"];
            var hdp2     = Request["Handicap2"];
            var odds     = Request["Odds"];
            var stake    = Request["Stake"];
            var betteam = Request["BetTeam"];

            return InsertBetdaq(username, betdaqid, oddsid, hdp1, hdp2, odds, stake, betteam, domain);
        }

        /// <summary>
        /// Inserts the BETDAQ.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="betdaqid">The BETDAQ id.</param>
        /// <param name="oddsid">The odds id.</param>
        /// <param name="hdp1">The HDP1 to insert.</param>
        /// <param name="hdp2">The HDP2 to insert.</param>
        /// <param name="odds">The odds to insert.</param>
        /// <param name="stake">The stake to insert.</param>
        /// <param name="betteam">The bet team.</param>
        /// <param name="domain">The domain.</param>
        /// <returns>System String.</returns>
        private string InsertBetdaq(
                                            string username, 
                                            string betdaqid,
                                            string oddsid,
                                            string hdp1,
                                            string hdp2,
                                            string odds,
                                            string stake,
                                            string betteam,
                                            string domain)
        {            
            var domainName = domain;
            if (string.IsNullOrEmpty(username) || username.ToLower() != "betdaq")
            {
                return "You are not authorized to take this action";
            }

            var ip = Utilities.GetIpAddressOfClient();
            var partner = new Partner(username, domainName, ip, string.Empty);
            var err = partner.IsAuthenticated();

            if (err.Id == 0)
            {
                var hash = new Hashtable
                             {                                    
                                 { "oddsid", oddsid }, 
                                 { "hdp1", hdp1 }, 
                                 { "hdp2", hdp2 }, 
                                 { "betteam", betteam },                                  
                                 { "odds", odds }, 
                                 { "stake", stake }, 
                                 { "betdaqid", betdaqid },
                                 { "bet_ip", ip }
                             };

                // check UM Time
                string startTime = string.Empty;
                string endTime = string.Empty;
                var ret = Utilities.GetDownTime(out startTime, out endTime) == true
                            ? DbHelper.ExecuteDbResult(ConfigurationManager.AppSettings["SPTestingUMTime"].ToString(), hash)
                            : DbHelper.ExecuteDbResult(Utilities.RealSp("SP.InsertBetDaq"), hash);

                int indexTable = ret.ResultSet.Tables.Count - 1;
                return ResultInsertBetDaq(ret.ResultSet.Tables[indexTable]);
            }

            return "Failed - Bad oddsId";
        }

        /// <summary>
        /// Results the insert bet DAQ.
        /// </summary>
        /// <param name="dt">The data table.</param>
        /// <returns>System String.</returns>
        private string ResultInsertBetDaq(DataTable dt)
        {
            DataRow dr = dt.Rows[0];
            switch (dr["ErrCode"].ToString())
            {
                case "0":
                    return "OK";                        
                case "1":
                    return "Failed - Bad oddsId";                        
                case "2":
                        return "OK Already";                        
                case "3":
                        return "Failed - Bad oddsId";                        
                default:
                        return "Failed - Bad oddsId";                        
            }
        }
    }
}
