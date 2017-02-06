namespace Fanex.OddsFeed.WebServiceLibrary.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security;
    using System.Text;
    using Common.Converters;
    using Common.DTO;
    using Filters;
    using Models;

    public class MatchBuilder : IMatchBuilder
    {
        private const int LiveStatus = 1;
        private const int DeadBallStatus = 0;
        private const int OddsTypeLiveMatch = 4;
        private const int OddsTypeDeadBallMatch = 2;

        private readonly IMatchFilter _matchFilter;

        public MatchBuilder()
        {
            _matchFilter = new MatchFilter();
        }

        public void AddMatchesNode(
            StringBuilder oddsFeedNode,
            OddsData oddsData,
            OddsFeedsModel model,
            UserLanguage userLanguage,
            DateTime lastUpdate)
        {
            var changetime = !string.IsNullOrEmpty(model.TimeStamp);
            var timeStamp = changetime ? int.Parse(model.TimeStamp) : 0;

            AppendBeginMatches(oddsFeedNode, oddsData.DeadballMatches, oddsData.LiveMatches, model, changetime, timeStamp, lastUpdate);

            if (HasSupportedLiveMatch(model.UserProfile, oddsData.LiveMatches))
            {
                AppendMatchNodes(oddsFeedNode, oddsData.LiveMatches, model, userLanguage, changetime, timeStamp);
            }

            if (HaveSupportedDeadBallMatch(model.UserProfile, oddsData.DeadballMatches))
            {
                AppendMatchNodes(oddsFeedNode, oddsData.DeadballMatches, model, userLanguage, changetime, timeStamp);
            }

            oddsFeedNode.Append("</matches>");
        }

        private static bool HasSupportedLiveMatch(UserProfile userProfile, IEnumerable<Match> matches)
        {
            return userProfile.OddsType != null 
                && userProfile.OddsType.Contains(OddsTypeLiveMatch)
                && matches.Any();
        }

        private static bool HaveSupportedDeadBallMatch(UserProfile userProfile, IEnumerable<Match> matches)
        {
            return userProfile.OddsType != null 
                && userProfile.OddsType.Contains(OddsTypeDeadBallMatch)
                && matches.Any();
        }

        private void AppendBeginMatches(
            StringBuilder oddsFeedNode,
            IEnumerable<Match> deadballMatches,
            IEnumerable<Match> liveMatches,
            OddsFeedsModel model,
            bool changeTime,
            int timeStamp,
            DateTime lastUpdate)
        {
            var matchesTimeStamp = GetMatchesTimeStamp(deadballMatches, liveMatches, model, changeTime, timeStamp);

            if (matchesTimeStamp > 0)
            {
                oddsFeedNode
                    .Append($"<matches lastupdate=\"{lastUpdate}\" timestamp=\"{matchesTimeStamp}\" oddstype=\"{GetOddsTypeName(model.UserProfile.OddRegion)}\">");
            }
            else
            {
                oddsFeedNode.Append("<matches>");
            }
        }

        private void AppendMatchNodes(
            StringBuilder oddsFeedNode,
            IList<Match> matches,
            OddsFeedsModel model,
            UserLanguage userLanguage,
            bool changeTime,
            int timeStamp)
        {
            if (model.IsSample)
            {
                AppendMatchNode(oddsFeedNode, matches.ElementAt(0), model, userLanguage, changeTime, timeStamp);
            }
            else
            {
                foreach (var match in matches)
                {
                    if (match.MatchOddsList.Count == 0)
                    {
                        continue;
                    }

                    var matchTimeStamp = match.MatchOddsList.Max(m => m.TimeStamp);

                    if (CheckMatchTimeStamp(matchTimeStamp, changeTime, timeStamp)
                        && _matchFilter.FilterMatch(match, model.UserProfile, model.UserQuery))
                    {
                        AppendMatchNode(oddsFeedNode, match, model, userLanguage, changeTime, timeStamp);
                    }
                }
            }
        }

        private void AppendMatchNode(
            StringBuilder oddsFeedNode,
            Match match,
            OddsFeedsModel model,
            UserLanguage userLanguage,
            bool changeTime,
            int timeStamp)
        {
            AppendBeginMatch(oddsFeedNode, match, userLanguage);
            AppendBookieNode(oddsFeedNode, match, model, changeTime, timeStamp);
            oddsFeedNode.Append("</match>");
        }

        private static void AppendBeginMatch(StringBuilder oddsFeedNode, Match match, UserLanguage userLanguage)
        {
            var isRunning = match.IsLive ? LiveStatus : DeadBallStatus;
            oddsFeedNode.Append($"<match id=\"{match.MatchId}\" ");

            AppendLeague(oddsFeedNode, match, userLanguage);
            AppendHomeTeam(oddsFeedNode, match, userLanguage);
            AppendAwayTeam(oddsFeedNode, match, userLanguage);

            oddsFeedNode.Append($"mt=\"{match.KickOfTime}\" sport=\"{SecurityElement.Escape(match.SportName)}\" running=\"{isRunning}\">");
        }

        private static void AppendLeague(StringBuilder oddsFeedNode, Match match, UserLanguage userLanguage)
        {
            if (userLanguage.IsSupportedEnglish)
            {
                oddsFeedNode.Append($"ge=\"{SecurityElement.Escape(match.LeagueEnglish)}\" ");
            }

            if (userLanguage.IsSupportedTraditionalChinese)
            {
                oddsFeedNode.Append($"gt=\"{SecurityElement.Escape(match.LeagueTraditionalChinese)}\" ");
            }

            if (userLanguage.IsSupportedSimplifiedChinese)
            {
                oddsFeedNode.Append($"gs=\"{SecurityElement.Escape(match.LeagueSimplifiedChinese)}\" ");
            }
        }

        private static void AppendHomeTeam(StringBuilder oddsFeedNode, Match match, UserLanguage userLanguage)
        {
            if (userLanguage.IsSupportedEnglish)
            {
                oddsFeedNode.Append($"he=\"{SecurityElement.Escape(match.HomeEnglish)}\" ");
            }

            if (userLanguage.IsSupportedTraditionalChinese)
            {
                oddsFeedNode.Append($"ht=\"{SecurityElement.Escape(match.HomeTraditionalChinese)}\" ");
            }

            if (userLanguage.IsSupportedSimplifiedChinese)
            {
                oddsFeedNode.Append($"hs=\"{SecurityElement.Escape(match.HomeSimplifiedChinese)}\" ");
            }
        }

        private static void AppendAwayTeam(StringBuilder oddsFeedNode, Match match, UserLanguage userLanguage)
        {
            if (userLanguage.IsSupportedEnglish)
            {
                oddsFeedNode.Append($"ae=\"{SecurityElement.Escape(match.AwayEnglish)}\" ");
            }

            if (userLanguage.IsSupportedTraditionalChinese)
            {
                oddsFeedNode.Append($"at=\"{SecurityElement.Escape(match.AwayTraditionalChinese)}\" ");
            }

            if (userLanguage.IsSupportedSimplifiedChinese)
            {
                oddsFeedNode.Append($"as=\"{SecurityElement.Escape(match.AwaySimplifiedChinese)}\" ");
            }
        }

        private void AppendBookieNode(
           StringBuilder matchesNode,
           Match match,
           OddsFeedsModel model,
           bool changeTime,
           int timeStamp)
        {
            matchesNode.Append($"<bookie name=\"{model.BookieName}\" simp=\"{model.BookieName}\" trad=\"{model.BookieName}\">");
            AppendOddNodeList(matchesNode, match.MatchOddsList, model, changeTime, timeStamp);
            matchesNode.Append("</bookie>");
        }

        private void AppendOddNodeList(
            StringBuilder oddsFeedNode,
            IEnumerable<MatchOdds> matchOddsNodes,
            OddsFeedsModel model,
            bool changeTime,
            int timeStamp)
        {
            foreach (var matchOddsNode in matchOddsNodes)
            {
                if (_matchFilter.FilterBetType(model.UserProfile, model.UserQuery, model.IsSample, changeTime, timeStamp, matchOddsNode))
                {
                    AppendOdd(oddsFeedNode, model.UserProfile, matchOddsNode);
                }
            }
        }

        private static void AppendOdd(StringBuilder oddsFeedNode, UserProfile userProfile, MatchOdds matchOddsNode)
        {
            oddsFeedNode.Append($"<odd t=\"{matchOddsNode.Tag}\"");

            foreach (var betChoice in matchOddsNode.BetChoices)
            {
                oddsFeedNode.Append($" {betChoice.Code}=\"{ConvertOdds(userProfile, betChoice)}\"");
            }

            oddsFeedNode.Append($" time=\"{matchOddsNode.Time}\" timestamp=\"{matchOddsNode.TimeStamp}\" />");
        }

        private static string ConvertOdds(UserProfile UserProfile, BetChoice betChoice)
        {
            return betChoice.NotConvert ? betChoice.Value : OddsConverter.ConvertOdds(UserProfile.OddRegion, Convert.ToDecimal(betChoice.Value)).ToString();
        }

        private int GetMatchesTimeStamp(
            IEnumerable<Match> deadballMatches,
            IEnumerable<Match> liveMatches,
            OddsFeedsModel model,
            bool changeTime,
            int timeStamp)
        {
            return Math.Max(
                GetMaxTimeStamp(deadballMatches, model, changeTime, timeStamp),
                GetMaxTimeStamp(liveMatches, model, changeTime, timeStamp));
        }

        private int GetMaxTimeStamp(
            IEnumerable<Match> matches,
            OddsFeedsModel model,
            bool changeTime,
            int timeStamp)
        {
            var maxTimeStamp = timeStamp;

            foreach (var match in matches)
            {
                if (match.MatchOddsList.Count == 0)
                {
                    continue;
                }

                var matchTimeStamp = match.MatchOddsList.Max(m => m.TimeStamp);

                if (CheckMatchTimeStamp(matchTimeStamp, changeTime, timeStamp) 
                    && _matchFilter.FilterMatch(match, model.UserProfile, model.UserQuery))
                {
                    maxTimeStamp = Math.Max(maxTimeStamp, matchTimeStamp);
                }
            }

            return maxTimeStamp;
        }

        private static bool CheckMatchTimeStamp(int matchTimeStamp, bool changeTime, int timeStamp)
        {
            return !changeTime || matchTimeStamp > timeStamp;
        }

        private static string GetOddsTypeName(int oddsTypeId)
        {
            var oddsTypeName = string.Empty;

            switch (oddsTypeId)
            {
                case 1:
                    oddsTypeName = "Malay";
                    break;
                case 2:
                    oddsTypeName = "HongKong";
                    break;
                case 3:
                    oddsTypeName = "Decimal";
                    break;
                case 4:
                    oddsTypeName = "Indo";
                    break;
                case 5:
                    oddsTypeName = "US";
                    break;

                default:
                    oddsTypeName = "Malay";
                    break;
            }

            return oddsTypeName;
        }
    }
}