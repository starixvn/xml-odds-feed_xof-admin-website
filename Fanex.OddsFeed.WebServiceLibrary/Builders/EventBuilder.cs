namespace Fanex.OddsFeed.WebServiceLibrary.Builders
{
    using Common.DTO;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Helpers;

    public class EventBuilder : IEventBuilder
    {
        public void AppendOutrightsNode(StringBuilder oddsFeedNode, IEnumerable<Outright> outrights)
        {
            oddsFeedNode.Append("<outrights>");

            foreach (var outright in outrights)
            {
                AppendEventNode(oddsFeedNode, outright);
            }

            oddsFeedNode.Append("</outrights>");
        }

        public void AppendMatchesNode(StringBuilder oddsFeedNode, IEnumerable<Match> matches)
        {
            var leagues = matches.GroupBy(league => league.LeagueId);

            oddsFeedNode.Append("<matches>");

            foreach (var league in leagues)
            {
                AppendLeagueNode(oddsFeedNode, league.Key, league.AsEnumerable());
            }

            oddsFeedNode.Append("</matches>");
        }

        private static void AppendLeagueNode(StringBuilder oddsFeedNode, int leagueId, IEnumerable<Match> matches)
        {
            var firstMatch = matches.First();
            oddsFeedNode.Append($"<league id=\"{leagueId}\" ge=\"{SecurityHelper.EscapeXML(firstMatch.LeagueEnglish)}\" gt=\"{SecurityHelper.EscapeXML(firstMatch.LeagueTraditionalChinese)}\" gs=\"{SecurityHelper.EscapeXML(firstMatch.LeagueSimplifiedChinese)}\" sport=\"{firstMatch.SportName}\">");

            foreach (var match in matches)
            {
                oddsFeedNode.Append($"<match id=\"{match.MatchId}\" he=\"{SecurityHelper.EscapeXML(match.HomeEnglish)}\" ht=\"{SecurityHelper.EscapeXML(match.HomeTraditionalChinese)}\" hs=\"{SecurityHelper.EscapeXML(match.HomeSimplifiedChinese)}\" ae=\"{SecurityHelper.EscapeXML(match.AwayEnglish)}\" at=\"{SecurityHelper.EscapeXML(match.AwayTraditionalChinese)}\" as=\"{SecurityHelper.EscapeXML(match.AwaySimplifiedChinese)}\" mt=\"{match.KickOfTime}\" running=\"{match.IsLive}\"/>");
            }

            oddsFeedNode.Append("</league>");
        }

        private static void AppendEventNode(StringBuilder oddsFeedNode, Outright outright)
        {
            oddsFeedNode.Append($"<event id=\"{outright.EventId}\" ge=\"{SecurityHelper.EscapeXML(outright.EventEnglish)}\" gt=\"{SecurityHelper.EscapeXML(outright.EventTraditionalChinese)}\" gs=\"{SecurityHelper.EscapeXML(outright.EventSimplifiedChinese)}\" mt=\"{outright.Time}\" sport=\"{outright.SportName}\">");

            foreach (var outrightOdds in outright.OutrightOddsList)
            {
                oddsFeedNode.Append($"<team te=\"{SecurityHelper.EscapeXML(outrightOdds.OddNameEnglish)}\" tt=\"{SecurityHelper.EscapeXML(outrightOdds.OddNameTraditionalChinese)}\" ts=\"{SecurityHelper.EscapeXML(outrightOdds.OddNameSimplifiedChinese)}\" />");
            }

            oddsFeedNode.Append("</event>");
        }
    }
}