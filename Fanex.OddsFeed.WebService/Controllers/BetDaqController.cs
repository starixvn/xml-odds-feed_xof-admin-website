// --------------------------------------------------------------------------
// <copyright file="BetDaqController.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>
namespace Fanex.OddsFeed.WebService.Controllers
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.OddsFeed.Library;
    using Fanex.OddsFeed.Library.Attributes;
    using Fanex.OddsFeed.WebService.Functions;

    /// <summary>
    /// Class BetDADController.
    /// </summary>    
    public class BetDaqController : Controller
    {
        #region Schedule
        /// <summary>
        /// Gets the schedule.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>System String.</returns>
        [Compress]
        public string GetSchedule(string userName)
        {
            if (userName.ToLower() != "betdaq")
            {
                return "You are not authorized to take this action.";
            }

            string startTime = string.Empty;
            string endTime = string.Empty;

            if (Utilities.GetDownTime(out startTime, out endTime))
            {
                return string.Empty;
            }

            var domain = Helper.DomainName(Request.ServerVariables);             
            var data = this.GetSchedule(userName, HttpContext.Request["t"], domain, HttpContext.Request["s"]);
            return data;
        }
        
        #endregion

        #region Odds
        /// <summary>
        /// Gets the odds.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>System String.</returns>
        [Compress]
        public string GetOdds(string userName)
        {
            if (userName.ToLower() != "betdaq")
            {
                return "You are not authorized to take this action.";
            }

            var startTime = string.Empty;
            var endTime = string.Empty;

            if (Utilities.GetDownTime(out startTime, out endTime))
            {
                return string.Empty;
            }

            var domain = Helper.DomainName(Request.ServerVariables);             
            var data = this.GetOdds(userName, HttpContext.Request["t"], domain, HttpContext.Request["s"]);
            return data;
        }
        
        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the schedule.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="domain">The domain.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>System String.</returns>
        private string GetSchedule(string username, string timestamp, string domain, string filter = null)
        {
            // ignore port number if it presents
            var domainName = domain;

            if (string.IsNullOrEmpty(username))
            {
                return string.Empty;
            }

            var ip = Utilities.GetIpAddressOfClient();
            var partner = new Partner(username, domainName, ip, timestamp);

            var err = partner.IsAuthenticated();

            if (err.Id == 0)
            {
                string result = string.Empty;
                string separateRow = "__";
                ////if (username.ToLower() != "betdaq")
                ////{
                ////    separateRow = System.Environment.NewLine;
                ////}

                if (string.IsNullOrEmpty(filter))
                {
                    // All
                    result = DbConnect.GetBetdaqScheduleData("Portal_BetDaq_Schedule").ResultSet.Tables[0].ToCSV(false, separateRow);
                }
                else if (filter.Equals("0") || filter.Equals("s"))
                {
                    // Soccer
                    result = DbConnect.GetBetdaqScheduleData("Portal_BetDaq_Schedule").ResultSet.Tables[0].Select("IsBasketBall = 0").ToCSV(separateRow);
                }
                else if (filter.Equals("1") || filter.Equals("b"))
                {
                    // BasketBall
                    result = DbConnect.GetBetdaqScheduleData("Portal_BetDaq_Schedule").ResultSet.Tables[0].Select("IsBasketBall = 1").ToCSV(separateRow);
                }
                else if (filter.Equals("2"))
                {
                    // Baseball
                    result = DbConnect.GetBetdaqScheduleData("Portal_BetDaq_Schedule").ResultSet.Tables[0].Select("IsBasketBall = 2").ToCSV(separateRow);
                }

                return result;
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the odds.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="domain">The domain.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>System String.</returns>
        private string GetOdds(string username, string timestamp, string domain, string filter = null)
        {
            // ignore port number if it presents
            var domainName = domain;

            if (string.IsNullOrEmpty(username))
            {
                return string.Empty;
            }

            var ip = Utilities.GetIpAddressOfClient();
            var partner = new Partner(username, domainName, ip, timestamp);

            var err = partner.IsAuthenticated();

            if (err.Id == 0)
            {                
                string separateRow = "__";
                return GetOddWithUserIsBetdaq(filter, separateRow);                   

                ////if (username.ToLower() == "betdaq")
                ////{
                ////    return GetOddWithUserIsBetdaq(filter, separateRow);                   
                ////}
                ////else
                ////{
                ////    separateRow = System.Environment.NewLine;
                ////    return GetOddWithOtherUser(filter, separateRow);                   
                ////}
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the odd with user is BETDAQ.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="separateRow">The separate row.</param>
        /// <returns>System String.</returns>
        private string GetOddWithUserIsBetdaq(string filter, string separateRow)
        {
            string result = string.Empty;
            DataTable oddsFeed_1 = DbConnect.GetBetdaqScheduleData("SP.GetOddsFeed").ResultSet.Tables[0];
            DataTable oddsFeed_2 = DbConnect.GetBetdaqScheduleData("SP.GetOddsFeed_2").ResultSet.Tables[0];
  
            if (string.IsNullOrEmpty(filter))
            {
                // All
                result = oddsFeed_1.Select().ToCSV_Odds(separateRow);
                result += oddsFeed_2.Select().ToCSV_Odds_2(separateRow);
            }
            else if (filter.Equals("0") || filter.Equals("s"))
            {
                // Soccer
                result = oddsFeed_1.Select("IsBasketBall = 0").ToCSV_Odds(separateRow);
                result += oddsFeed_2.Select("IsBasketBall = 0").ToCSV_Odds_2(separateRow);
            }
            else if (filter.Equals("1") || filter.Equals("b"))
            {
                // BasketBall
                result = oddsFeed_1.Select("IsBasketBall = 1").ToCSV_Odds(separateRow);
                result += oddsFeed_2.Select("IsBasketBall = 1").ToCSV_Odds_2(separateRow);
            }
            else if (filter.Equals("2"))
            {
                // Baseball
                result = oddsFeed_1.Select("IsBasketBall = 2").ToCSV_Odds(separateRow);
                result += oddsFeed_1.Select("IsBasketBall = 2").ToCSV_Odds_2(separateRow);
            }

            return result;
        }

        /// <summary>
        /// Gets the odd with other user.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="separateRow">The separate row.</param>
        /// <returns>System String.</returns>
        private string GetOddWithOtherUser(string filter, string separateRow)
        {
            string result = string.Empty;
            DataTable oddsFeed_1 = DbConnect.GetBetdaqScheduleData("SP.GetOddsFeed").ResultSet.Tables[0];
            DataTable oddsFeed_2 = DbConnect.GetBetdaqScheduleData("SP.GetOddsFeed_2").ResultSet.Tables[0];

            if (string.IsNullOrEmpty(filter))
            {
                // All
                result = oddsFeed_1.Select(" ").FooBetToCSV_Odds(separateRow);
                result += oddsFeed_2.Select().FooBetToCSV_Odds_2(separateRow);
            }
            else if (filter.Equals("0") || filter.Equals("s"))
            {
                // Soccer
                result = oddsFeed_1.Select("IsBasketBall = 0").FooBetToCSV_Odds(separateRow);
                result += oddsFeed_2.Select("IsBasketBall = 0").FooBetToCSV_Odds_2(separateRow);
            }
            else if (filter.Equals("1") || filter.Equals("b"))
            {
                // BasketBall
                result = oddsFeed_1.Select("IsBasketBall = 1").FooBetToCSV_Odds(separateRow);
                result += oddsFeed_2.Select("IsBasketBall = 1").FooBetToCSV_Odds_2(separateRow);
            }
            else if (filter.Equals("2"))
            {
                // Baseball
                result = oddsFeed_1.Select("IsBasketBall = 2").FooBetToCSV_Odds(separateRow);
                result += oddsFeed_2.Select("IsBasketBall = 2").FooBetToCSV_Odds_2(separateRow);
            }

            return result;
        }

        #endregion
    }
}
