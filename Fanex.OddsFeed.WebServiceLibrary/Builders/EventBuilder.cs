namespace Fanex.OddsFeed.WebServiceLibrary.Builders
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Common.DTO;

    public class EventBuilder: IEventBuilder
    {
        public void AppendOutrightsNode(StringBuilder oddsFeedNode, IEnumerable<Outright> outrights)
        {
            AppendBeginOutrights(oddsFeedNode);

            foreach (var outright in outrights)
            {
                AppendEventNode(oddsFeedNode, outright);
            }

            AppendEndOutrights(oddsFeedNode);
        }

        public void AppendMatchesNode(StringBuilder oddsFeedNode, IEnumerable<Match> matches)
        {
            var leagues = matches.GroupBy(league => league.LeagueId);

            AppendBeginMatches(oddsFeedNode);

            foreach (var league in leagues)
            {
                AppendLeagueNode(oddsFeedNode, league.Key, league.AsEnumerable());
            }

            AppendEndMatches(oddsFeedNode);
        }

        private void AppendLeagueNode(StringBuilder oddsFeedNode, int leagueId, IEnumerable<Match> matches)
        {
            AppendBeginLeague(oddsFeedNode, leagueId, matches.First());

            foreach (var match in matches)
            {
                AppendMatch(oddsFeedNode, match);
            }

            AppendEndLeague(oddsFeedNode);
        }

        private void AppendBeginOutrights(StringBuilder oddsFeedNode)
        {
            oddsFeedNode.Append("<outrights>");
        }

        private void AppendEndOutrights(StringBuilder oddsFeedNode)
        {
            oddsFeedNode.Append("</outrights>");
        }

        private void AppendEventNode(StringBuilder oddsFeedNode, Outright outright)
        {
            AppendBeginEvent(oddsFeedNode, outright);

            foreach (var outrightOddsNode in outright.OutrightOddsList)
            {
                AppendTeam(oddsFeedNode, outrightOddsNode);
            }

            AppendEndEvent(oddsFeedNode);
        }

        public void AppendBeginEvent(StringBuilder oddsFeedNode, Outright outright)
        {
            oddsFeedNode.Append($"<event id=\"{outright.EventId}\" ge=\"{outright.EventEnglish}\" gt=\"{outright.EventTraditionalChinese}\" gs=\"{outright.EventSimplifiedChinese}\" mt=\"{outright.Time}\" sport=\"{outright.SportName}\">");
        }

        private void AppendEndEvent(StringBuilder oddsFeedNode)
        {
            oddsFeedNode.Append("</event>");
        }

        private void AppendTeam(StringBuilder oddsFeedNode, OutrightOdds outrightOdds)
        {
            oddsFeedNode.Append($"<team te=\"{outrightOdds.OddNameEnglish}\" tt=\"{outrightOdds.OddNameTraditionalChinese}\" ts=\"{outrightOdds.OddNameSimplifiedChinese}\" />");
        }

        private void AppendBeginMatches(StringBuilder oddsFeedNode)
        {
            oddsFeedNode.Append("<matches>");
        }

        private void AppendEndMatches(StringBuilder oddsFeedNode)
        {
            oddsFeedNode.Append("</matches>");
        }

        private void AppendBeginLeague(StringBuilder oddsFeedNode, int leagueId, Match match)
        {
            oddsFeedNode.Append($"<league id=\"{leagueId}\" ge=\"{match.LeagueEnglish}\" gt=\"{match.LeagueTraditionalChinese}\" gs=\"{match.LeagueSimplifiedChinese}\" sport=\"{match.SportName}\">");
        }

        private void AppendEndLeague(StringBuilder oddsFeedNode)
        {
            oddsFeedNode.Append("</league>");
        }

        private void AppendMatch(StringBuilder oddsFeedNode, Match match)
        {
            oddsFeedNode.Append($"<match id=\"{match.MatchId}\" he=\"{match.HomeEnglish}\" ht=\"{match.HomeTraditionalChinese}\" hs=\"{match.HomeSimplifiedChinese}\" ae=\"{match.AwayEnglish}\" at=\"{match.AwayTraditionalChinese}\" as=\"{match.AwaySimplifiedChinese}\" mt=\"{match.KickOfTime}\" running=\"{match.IsLive}\"/>");
        }
    }
}
