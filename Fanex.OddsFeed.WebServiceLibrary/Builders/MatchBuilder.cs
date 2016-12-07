namespace Fanex.OddsFeed.WebServiceLibrary.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Common.Converters;
    using Common.DTO;
    using Filters;

    public class MatchBuilder : IMatchBuilder
    {
        private readonly IMatchFilter _matchFilter;

        public MatchBuilder()
        {
            _matchFilter = new MatchFilter();
        }

        public void AppendMatchesNode(
            StringBuilder oddsFeedNode,
            UserProfile userProfile,
            UserQuery userQuery,
            IList<Match> deadballMatches,
            IList<Match> liveMatches,
            string bookieName,
            DateTime lastUpdate,
            bool isSample)
        {
            AppendBeginMatches(oddsFeedNode, userProfile, deadballMatches, liveMatches, lastUpdate);

            if (deadballMatches.Any())
            {
                AssignMatchNodes(oddsFeedNode, userProfile, userQuery, deadballMatches, bookieName, isSample);
            }

            if (liveMatches.Any())
            {
                AssignMatchNodes(oddsFeedNode, userProfile, userQuery, liveMatches, bookieName, isSample);
            }

            AppendEndMatches(oddsFeedNode);
        }

        private void AppendBeginMatches(
            StringBuilder oddsFeedNode,
            UserProfile userProfile,
            IEnumerable<Match> deadballMatches,
            IEnumerable<Match> liveMatches,
            DateTime lastUpdate)
        {
            if (deadballMatches.Any() || liveMatches.Any())
            {
                var matchesTimeStamp = GetMatchesTimeStamp(deadballMatches, liveMatches);

                oddsFeedNode
                    .Append($"<matches lastupdate=\"{lastUpdate}\" timestamp=\"{matchesTimeStamp}\" oddstype=\"{GetOddsTypeName(userProfile.OddRegion)}\">");
            }
            else
            {
                oddsFeedNode.Append("<matches>");
            }
        }

        private void AppendEndMatches(StringBuilder oddsFeedNode)
        {
            oddsFeedNode.Append("</matches>");
        }

        private void AssignMatchNodes(
            StringBuilder oddsFeedNode,
            UserProfile userProfile,
            UserQuery userQuery,
            IEnumerable<Match> matches,
            string bookieName,
            bool isSample)
        {
            if (isSample)
            {
                AppendMatchNode(oddsFeedNode, userProfile, userQuery, matches.ElementAt(0), bookieName, isSample);
            }
            else
            {
                foreach (var match in matches)
                {
                    AppendMatchNode(oddsFeedNode, userProfile, userQuery, match, bookieName, isSample);
                }
            }
        }

        private void AppendMatchNode(
            StringBuilder oddsFeedNode,
            UserProfile userProfile,
            UserQuery userQuery,
            Match match,
            string bookieName,
            bool isSample)
        {
            AppendBeginMatch(oddsFeedNode, match);
            AppendBookieNode(oddsFeedNode, userProfile, userQuery, match, bookieName, isSample);
            AppendEndMatch(oddsFeedNode);
        }

        private void AppendBeginMatch(StringBuilder oddsFeedNode, Match match)
        {
            oddsFeedNode
                .Append($"<match id=\"{match.MatchId}\" ge=\"{match.LeagueEnglish}\" gt=\"{match.LeagueTraditionalChinese}\" gs=\"{match.LeagueSimplifiedChinese}\" he=\"{match.HomeEnglish}\" ht=\"{match.HomeTraditionalChinese}\" hs=\"{match.HomeSimplifiedChinese}\" ae=\"{match.AwayEnglish}\" at=\"{match.AwayTraditionalChinese}\" as=\"{match.AwaySimplifiedChinese}\" mt=\"{match.KickOfTime}\" sport=\"{match.SportName}\" running=\"{match.IsLive}\">");
        }

        private void AppendEndMatch(StringBuilder oddsFeedNode)
        {
            oddsFeedNode.Append("</match>");
        }

        private void AppendBookieNode(
           StringBuilder matchesNode,
           UserProfile userProfile,
           UserQuery userQuery,
           Match match,
           string bookieName,
           bool isSample)
        {
            AppendBeginBookie(matchesNode, bookieName);
            AppendOddNodeList(matchesNode, userProfile, userQuery, match.MatchOddsList, isSample);
            AppendEndBookie(matchesNode);
        }

        private void AppendBeginBookie(StringBuilder oddsFeedNode, string bookieName)
        {
            oddsFeedNode.Append($"<bookie name=\"{bookieName}\" simp=\"{bookieName}\" trad=\"{bookieName}\">");
        }

        private void AppendEndBookie(StringBuilder oddsFeedNode)
        {
            oddsFeedNode.Append("</bookie>");
        }

        private void AppendOddNodeList(
            StringBuilder oddsFeedNode,
            UserProfile userProfile,
            UserQuery userQuery,
            IEnumerable<MatchOdds> matchOddsNodes,
            bool isSample)
        {
            foreach (var matchOddsNode in matchOddsNodes)
            {
                if (_matchFilter.FilterBetType(userProfile, userQuery, isSample, matchOddsNode))
                {
                    AppendOdd(oddsFeedNode, userProfile, matchOddsNode);
                }
            }
        }

        private void AppendOdd(StringBuilder oddsFeedNode, UserProfile userProfile, MatchOdds matchOddsNode)
        {
            oddsFeedNode.Append($"<odd t=\"{matchOddsNode.Tag}\"");

            foreach (var betChoice in matchOddsNode.BetChoices)
            {
                oddsFeedNode.Append($"{betChoice.Code}=\"{ConvertOdds(userProfile, betChoice)}\" ");
            }

            oddsFeedNode.Append($"time=\"{matchOddsNode.Time}\" timestamp=\"{matchOddsNode.TimeStamp}\" />");
        }

        private static string ConvertOdds(UserProfile UserProfile, BetChoice betChoice)
        {
            if (betChoice.NotConvert)
            {
                return betChoice.Value;
            }

            return OddsConverter.ConvertOdds(UserProfile.OddRegion, Convert.ToDecimal(betChoice.Value)).ToString();
        }

        private static int GetMatchesTimeStamp(IEnumerable<Match> deadballMatches, IEnumerable<Match> liveMatches)
        {
            var deadBallTimeStamp = 0;
            var liveTimeStamp = 0;

            if (deadballMatches.Any())
            {
                deadBallTimeStamp = deadballMatches.Max(match => match.TimeStamp);
            }

            if (liveMatches.Any())
            {
                liveTimeStamp = liveMatches.Max(match => match.TimeStamp);
            }

            return Math.Max(deadBallTimeStamp, liveTimeStamp);
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
