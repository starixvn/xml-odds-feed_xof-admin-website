namespace Fanex.OddsFeed.WebServiceLibrary.Builders
{
    using Common.Converters;
    using Common.DTO;
    using Filters;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Models;
    using System.Security;

    public class MatchBuilder : IMatchBuilder
    {
        private const int LIVE = 1;
        private const int DEADBALL = 0;

        private readonly IMatchFilter _matchFilter;

        public MatchBuilder()
        {
            _matchFilter = new MatchFilter();
        }

        public void AppendMatchesNode(
            StringBuilder oddsFeedNode,
            IList<Match> deadballMatches,
            IList<Match> liveMatches,
            OddsFeedsModel model,
            DateTime lastUpdate)
        {
            AppendBeginMatches(oddsFeedNode, model.UserProfile, deadballMatches, liveMatches, lastUpdate);

            if (deadballMatches.Any())
            {
                AssignMatchNodes(oddsFeedNode, deadballMatches, model);
            }

            if (liveMatches.Any())
            {
                AssignMatchNodes(oddsFeedNode, liveMatches, model);
            }

            oddsFeedNode.Append("</matches>");
        }

        private static void AppendBeginMatches(
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

        private void AssignMatchNodes(
            StringBuilder oddsFeedNode,
            IEnumerable<Match> matches,
            OddsFeedsModel model)
        {
            if (model.IsSample)
            {
                AppendMatchNode(oddsFeedNode, matches.ElementAt(0), model);
            }
            else
            {
                foreach (var match in matches)
                {
                    AppendMatchNode(oddsFeedNode, match, model);
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
            var isRunning = match.IsLive ? LIVE : DEADBALL;
            oddsFeedNode
                .Append($"<match id=\"{match.MatchId}\" ge=\"{SecurityElement.Escape(match.LeagueEnglish)}\" gt=\"{SecurityElement.Escape(match.LeagueTraditionalChinese)}\" gs=\"{SecurityElement.Escape(match.LeagueSimplifiedChinese)}\" ");
            oddsFeedNode
                .Append($"he=\"{SecurityElement.Escape(match.HomeEnglish)}\" ht=\"{SecurityElement.Escape(match.HomeTraditionalChinese)}\" hs=\"{SecurityElement.Escape(match.HomeSimplifiedChinese)}\" ");
            oddsFeedNode
                .Append($"ae=\"{SecurityElement.Escape(match.AwayEnglish)}\" at=\"{SecurityElement.Escape(match.AwayTraditionalChinese)}\" as=\"{SecurityElement.Escape(match.AwaySimplifiedChinese)}\" mt=\"{match.KickOfTime}\" sport=\"{match.SportName}\" running=\"{isRunning}\">");
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