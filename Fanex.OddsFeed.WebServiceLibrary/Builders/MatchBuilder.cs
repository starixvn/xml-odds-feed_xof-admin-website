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
            DateTime lastUpdate)
        {
            var changetime = !string.IsNullOrEmpty(model.TimeStamp);
            var timeStamp = changetime ? int.Parse(model.TimeStamp) : 0;

            AppendBeginMatches(oddsFeedNode, oddsData.DeadballMatches, oddsData.LiveMatches, model, changetime, timeStamp, lastUpdate);

            if (HasSupportedLiveMatch(model.UserProfile, oddsData.LiveMatches))
            {
                AppendMatchNodes(oddsFeedNode, oddsData.LiveMatches, model, changetime, timeStamp);
            }

            if (HaveSupportedDeadBallMatch(model.UserProfile, oddsData.DeadballMatches))
            {
                AppendMatchNodes(oddsFeedNode, oddsData.DeadballMatches, model, changetime, timeStamp);
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
            bool changeTime,
            int timeStamp)
        {
            if (model.IsSample)
            {
                AppendMatchNode(oddsFeedNode, matches.ElementAt(0), model);
            }
            else
            {
                foreach (var match in matches)
                {
                    if (_matchFilter.FilterMatch(match, model.UserProfile, model.UserQuery, changeTime, timeStamp))
                    {
                        AppendMatchNode(oddsFeedNode, match, model);
                    }
                }
            }
        }

        private void AppendMatchNode(
            StringBuilder oddsFeedNode,
            Match match,
            OddsFeedsModel model)
        {
            AppendBeginMatch(oddsFeedNode, match);
            AppendBookieNode(oddsFeedNode, match, model);
            oddsFeedNode.Append("</match>");
        }

        private static void AppendBeginMatch(StringBuilder oddsFeedNode, Match match)
        {
            var isRunning = match.IsLive ? LiveStatus : DeadBallStatus;
            oddsFeedNode
                .Append($"<match id=\"{match.MatchId}\" ge=\"{SecurityElement.Escape(match.LeagueEnglish)}\" gt=\"{SecurityElement.Escape(match.LeagueTraditionalChinese)}\" gs=\"{SecurityElement.Escape(match.LeagueSimplifiedChinese)}\" ");
            oddsFeedNode
                .Append($"he=\"{SecurityElement.Escape(match.HomeEnglish)}\" ht=\"{SecurityElement.Escape(match.HomeTraditionalChinese)}\" hs=\"{SecurityElement.Escape(match.HomeSimplifiedChinese)}\" ");
            oddsFeedNode
                .Append($"ae=\"{SecurityElement.Escape(match.AwayEnglish)}\" at=\"{SecurityElement.Escape(match.AwayTraditionalChinese)}\" as=\"{SecurityElement.Escape(match.AwaySimplifiedChinese)}\" mt=\"{match.KickOfTime}\" sport=\"{SecurityElement.Escape(match.SportName)}\" running=\"{isRunning}\">");
        }

        private void AppendBookieNode(
           StringBuilder matchesNode,
           Match match,
           OddsFeedsModel model)
        {
            matchesNode.Append($"<bookie name=\"{model.BookieName}\" simp=\"{model.BookieName}\" trad=\"{model.BookieName}\">");
            AppendOddNodeList(matchesNode, match.MatchOddsList, model);
            matchesNode.Append("</bookie>");
        }

        private void AppendOddNodeList(
            StringBuilder oddsFeedNode,
            IEnumerable<MatchOdds> matchOddsNodes,
            OddsFeedsModel model)
        {
            foreach (var matchOddsNode in matchOddsNodes)
            {
                if (_matchFilter.FilterBetType(model.UserProfile, model.UserQuery, model.IsSample, matchOddsNode))
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
            var maxTimeStamp = 0;

            foreach (var match in matches)
            {
                if (_matchFilter.FilterMatch(match, model.UserProfile, model.UserQuery, changeTime, timeStamp))
                {
                    maxTimeStamp = Math.Max(timeStamp, match.TimeStamp);
                }
            }

            return maxTimeStamp;
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