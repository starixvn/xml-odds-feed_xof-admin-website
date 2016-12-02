namespace Fanex.OddsFeed.WebService.Controllers
{
    using Fanex.OddsFeed.WebService.Functions;
    using OddsFeed.Common;
    using WebServiceLibrary.Criteria;

    public class BetDaqController : BaseController
    {
        public string GetSchedule(string userName)
        {
            if (userName.ToLower() != "betdaq")
            {
                return "You are not authorized to take this action.";
            }
            
            if (IsDownTime())
            {
                return string.Empty;
            }
       
            return GetSchedule(userName, Helper.DomainName(Request.ServerVariables), HttpContext.Request["s"]);
        }
        
        public string GetOdds(string userName)
        {
            if (userName.ToLower() != "betdaq")
            {
                return "You are not authorized to take this action.";
            }

            if (IsDownTime())
            {
                return string.Empty;
            }

            return GetOdds(userName, Helper.DomainName(Request.ServerVariables), HttpContext.Request["s"]);
        }
       
        private string GetSchedule(string username, string domainName, string filter = null)
        {
            if (string.IsNullOrEmpty(username))
            {
                return string.Empty;
            }
            
            var queryResult = GetAuthenticatedUserProfile(new CheckPermissionCriteria
            {
                UserName = username,
                DomainName = domainName,
                Ip = GetClientIpAddress()
            });

            if (queryResult.Error.Id == 0)
            {
                var separateRow = "__";

                if (string.IsNullOrEmpty(filter))
                {
                    return DbAccess.GetBetdaqScheduleData("Portal_BetDaq_Schedule").ResultSet.Tables[0].ToCSV(false, separateRow);
                }
                else if (filter.Equals("0") || filter.Equals("s"))
                {
                    return DbAccess.GetBetdaqScheduleData("Portal_BetDaq_Schedule").ResultSet.Tables[0].Select("IsBasketBall = 0").ToCSV(separateRow);
                }
                else if (filter.Equals("1") || filter.Equals("b"))
                {
                    return DbAccess.GetBetdaqScheduleData("Portal_BetDaq_Schedule").ResultSet.Tables[0].Select("IsBasketBall = 1").ToCSV(separateRow);
                }
                else if (filter.Equals("2"))
                {
                    return DbAccess.GetBetdaqScheduleData("Portal_BetDaq_Schedule").ResultSet.Tables[0].Select("IsBasketBall = 2").ToCSV(separateRow);
                }
            }

            return string.Empty;
        }

        private string GetOdds(string username, string domainName, string filter = null)
        {
            if (string.IsNullOrEmpty(username))
            {
                return string.Empty;
            }

            var queryResult = GetAuthenticatedUserProfile(new CheckPermissionCriteria
            {
                UserName = username,
                DomainName = domainName,
                Ip = GetClientIpAddress()
            });
            
            if (queryResult.Error.Id == 0)
            {                
                var separateRow = "__";

                return GetOddWithUserIsBetdaq(filter, separateRow);                   
            }

            return string.Empty;
        }

        private string GetOddWithUserIsBetdaq(string filter, string separateRow)
        {
            var result = string.Empty;
            var oddsFeed_1 = DbAccess.GetBetdaqScheduleData("SP.GetOddsFeed").ResultSet.Tables[0];
            var oddsFeed_2 = DbAccess.GetBetdaqScheduleData("SP.GetOddsFeed_2").ResultSet.Tables[0];
  
            if (string.IsNullOrEmpty(filter))
            {
                result = oddsFeed_1.Select().ToCSV_Odds(separateRow);
                result += oddsFeed_2.Select().ToCSV_Odds_2(separateRow);
            }
            else if (filter.Equals("0") || filter.Equals("s"))
            {
                result = oddsFeed_1.Select("IsBasketBall = 0").ToCSV_Odds(separateRow);
                result += oddsFeed_2.Select("IsBasketBall = 0").ToCSV_Odds_2(separateRow);
            }
            else if (filter.Equals("1") || filter.Equals("b"))
            {
                result = oddsFeed_1.Select("IsBasketBall = 1").ToCSV_Odds(separateRow);
                result += oddsFeed_2.Select("IsBasketBall = 1").ToCSV_Odds_2(separateRow);
            }
            else if (filter.Equals("2"))
            {
                result = oddsFeed_1.Select("IsBasketBall = 2").ToCSV_Odds(separateRow);
                result += oddsFeed_1.Select("IsBasketBall = 2").ToCSV_Odds_2(separateRow);
            }

            return result;
        }

        private string GetOddWithOtherUser(string filter, string separateRow)
        {
            var result = string.Empty;
            var oddsFeed_1 = DbAccess.GetBetdaqScheduleData("SP.GetOddsFeed").ResultSet.Tables[0];
            var oddsFeed_2 = DbAccess.GetBetdaqScheduleData("SP.GetOddsFeed_2").ResultSet.Tables[0];

            if (string.IsNullOrEmpty(filter))
            {
                result = oddsFeed_1.Select(" ").FooBetToCSV_Odds(separateRow);
                result += oddsFeed_2.Select().FooBetToCSV_Odds_2(separateRow);
            }
            else if (filter.Equals("0") || filter.Equals("s"))
            {
                result = oddsFeed_1.Select("IsBasketBall = 0").FooBetToCSV_Odds(separateRow);
                result += oddsFeed_2.Select("IsBasketBall = 0").FooBetToCSV_Odds_2(separateRow);
            }
            else if (filter.Equals("1") || filter.Equals("b"))
            {
                result = oddsFeed_1.Select("IsBasketBall = 1").FooBetToCSV_Odds(separateRow);
                result += oddsFeed_2.Select("IsBasketBall = 1").FooBetToCSV_Odds_2(separateRow);
            }
            else if (filter.Equals("2"))
            {
                result = oddsFeed_1.Select("IsBasketBall = 2").FooBetToCSV_Odds(separateRow);
                result += oddsFeed_2.Select("IsBasketBall = 2").FooBetToCSV_Odds_2(separateRow);
            }

            return result;
        }
    }
}
