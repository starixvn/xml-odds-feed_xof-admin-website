namespace Fanex.OddsFeed.WebServiceLibrary.Services
{
    using System.Collections;
    using Common;
    using Common.Configurations;
    using Common.Helpers;
    using Extensions;
    using Fanex.Common;
    using Fanex.Data.DNAide;

    public class BetDaqService : IBetDaqService
    {
        private const string SeparateRow = "__";
        private const string BasketBall0 = "0";
        private const string BasketBall1 = "1";
        private const string BasketBall2 = "2";

        public string GetSchedule(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return DbAccess.GetBetdaqScheduleData("Portal_BetDaq_Schedule").Tables[0].ToCSV(false, SeparateRow);
            }
            else if (filter.Equals(BasketBall0) || filter.Equals("s"))
            {
                return DbAccess.GetBetdaqScheduleData("Portal_BetDaq_Schedule").Tables[0].Select("IsBasketBall = 0").ToCSV(SeparateRow);
            }
            else if (filter.Equals(BasketBall1) || filter.Equals("b"))
            {
                return DbAccess.GetBetdaqScheduleData("Portal_BetDaq_Schedule").Tables[0].Select("IsBasketBall = 1").ToCSV(SeparateRow);
            }
            else if (filter.Equals(BasketBall2))
            {
                return DbAccess.GetBetdaqScheduleData("Portal_BetDaq_Schedule").Tables[0].Select("IsBasketBall = 2").ToCSV(SeparateRow);
            }

            return string.Empty;
        }

        public string GetOddWithUserIsBetdaq(string filter)
        {
            var result = string.Empty;
            var oddsFeed_1 = DbAccess.GetBetdaqScheduleData("SP.GetOddsFeed").Tables[0];
            var oddsFeed_2 = DbAccess.GetBetdaqScheduleData("SP.GetOddsFeed_2").Tables[0];

            if (string.IsNullOrEmpty(filter))
            {
                result = oddsFeed_1.Select().ToCSV_Odds(SeparateRow);
                result += oddsFeed_2.Select().ToCSV_Odds_2(SeparateRow);
            }
            else if (filter.Equals(BasketBall0) || filter.Equals("s"))
            {
                result = oddsFeed_1.Select("IsBasketBall = 0").ToCSV_Odds(SeparateRow);
                result += oddsFeed_2.Select("IsBasketBall = 0").ToCSV_Odds_2(SeparateRow);
            }
            else if (filter.Equals(BasketBall1) || filter.Equals("b"))
            {
                result = oddsFeed_1.Select("IsBasketBall = 1").ToCSV_Odds(SeparateRow);
                result += oddsFeed_2.Select("IsBasketBall = 1").ToCSV_Odds_2(SeparateRow);
            }
            else if (filter.Equals(BasketBall2))
            {
                result = oddsFeed_1.Select("IsBasketBall = 2").ToCSV_Odds(SeparateRow);
                result += oddsFeed_1.Select("IsBasketBall = 2").ToCSV_Odds_2(SeparateRow);
            }

            return result;
        }

        public int InsertBetDaq(Hashtable spParams, bool isDownTime)
        {
            var result = isDownTime
                        ? DbHelper.ExecuteDbResult(AppSettings.SpTestingUMTime, spParams)
                        : DbHelper.ExecuteDbResult(SpHelper.RealSp("SP.InsertBetDaq"), spParams);
            var indexTable = result.ResultSet.Tables.Count - 1;

            return Converter.ToInt(result.ResultSet.Tables[indexTable].Rows[0]["ErrCode"]);
        }
    }
}
