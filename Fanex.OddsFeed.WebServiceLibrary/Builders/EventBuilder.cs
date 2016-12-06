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
            oddsFeedNode.AppendFormat("<event id=\"{0}\" ge=\"{1}\" gt=\"{2}\" gs=\"{3}\" mt=\"{4}\" sport=\"{5}\">",
                outright.EventId,
                outright.EventEnglish,
                outright.EventTraditionalChinese,
                outright.EventSimplifiedChinese,
                outright.Time,
                outright.SportName);
        }

        private void AppendEndEvent(StringBuilder oddsFeedNode)
        {
            oddsFeedNode.Append("</event>");
        }

        private void AppendTeam(StringBuilder oddsFeedNode, OutrightOdds outrightOdds)
        {
            oddsFeedNode.AppendFormat("<team te=\"{0}\" tt=\"{1}\" ts=\"{2}\" />",
                outrightOdds.OddNameEnglish,
                outrightOdds.OddNameTraditionalChinese,
                outrightOdds.OddNameSimplifiedChinese);
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
            oddsFeedNode.AppendFormat("<league id=\"{0}\" ge=\"{1}\" gt=\"{2}\" gs=\"{3}\" sport=\"{4}\">",
                leagueId,
                match.LeagueEnglish,
                match.LeagueTraditionalChinese,
                match.LeagueSimplifiedChinese,
                match.SportName);
        }

        private void AppendEndLeague(StringBuilder oddsFeedNode)
        {
            oddsFeedNode.Append("</league>");
        }

        private void AppendMatch(StringBuilder oddsFeedNode, Match match)
        {
            oddsFeedNode.AppendFormat("<match id=\"{0}\" he=\"\" ht=\"\" hs=\"\" ae=\"\" at=\"\" as=\"\" mt=\"\" running=\"\"/>",
                match.MatchId,
                match.HomeEnglish,
                match.HomeTraditionalChinese,
                match.HomeSimplifiedChinese,
                match.AwayEnglish,
                match.AwayTraditionalChinese,
                match.AwaySimplifiedChinese,
                match.KickOfTime,
                match.IsLive);
        }
    }
}
