namespace Fanex.OddsFeed.WebService.Controllers
{
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Data;
    using Fanex.Data.DNAide;
    using OddsFeed.Common.Configurations;
    using OddsFeed.Common.Helpers;
    using WebServiceLibrary.Criteria;
    using WebServiceLibrary.Helpers;

    public class InsertBetdaqController : BaseController
    {
        public string Index(string username)
        {
            if (!Convert.ToBoolean(AppSettings.IsDisableUMTimeFlag))
            {
                if (username.ToLower() != "betdaq")
                {
                    return "You are not authorized to take this action.";
                }
                
                if (IsDownTime())
                {
                    return "Failed - Bad oddsId";
                }
            }

            var domain = RequestHelper.DomainName(Request.ServerVariables);                         
            
            var betdaqid = Request["RequestId"];
            var oddsid   = Request["OddsId"];
            var hdp1     = Request["Handicap1"];
            var hdp2     = Request["Handicap2"];
            var odds     = Request["Odds"];
            var stake    = Request["Stake"];
            var betteam = Request["BetTeam"];

            return InsertBetdaq(username, betdaqid, oddsid, hdp1, hdp2, odds, stake, betteam, domain);
        }

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

            var ipAddress = GetClientIpAddress();
            var authenticatedPartner = GetAuthenticatedPartner(new CheckPermissionCriteria
            {
                UserName = username,
                DomainName = domainName,
                Ip = ipAddress
            });

            if (string.IsNullOrEmpty(authenticatedPartner.Error))
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
                                 { "bet_ip", ipAddress }
                             };
                
                var ret = IsDownTime()
                            ? DbHelper.ExecuteDbResult(ConfigurationManager.AppSettings["SPTestingUMTime"].ToString(), hash)
                            : DbHelper.ExecuteDbResult(SpHelper.RealSp("SP.InsertBetDaq"), hash);

                var indexTable = ret.ResultSet.Tables.Count - 1;
                return ResultInsertBetDaq(ret.ResultSet.Tables[indexTable]);
            }

            return "Failed - Bad oddsId";
        }

        private static string ResultInsertBetDaq(DataTable dt)
        {
            var dr = dt.Rows[0];
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
