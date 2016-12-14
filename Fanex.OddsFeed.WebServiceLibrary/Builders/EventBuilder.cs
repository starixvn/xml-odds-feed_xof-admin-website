namespace Fanex.OddsFeed.WebServiceLibrary.Builders
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security;
    using System.Text;
    using Common.DTO;

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
            oddsFeedNode.Append($"<league id=\"{leagueId}\" ge=\"{SecurityElement.Escape(firstMatch.LeagueEnglish)}\" gt=\"{SecurityElement.Escape(firstMatch.LeagueTraditionalChinese)}\" gs=\"{SecurityElement.Escape(firstMatch.LeagueSimplifiedChinese)}\" sport=\"{SecurityElement.Escape(firstMatch.SportName)}\">");

            foreach (var match in matches)
            {
                oddsFeedNode.Append($"<match id=\"{match.MatchId}\" he=\"{SecurityElement.Escape(match.HomeEnglish)}\" ht=\"{SecurityElement.Escape(match.HomeTraditionalChinese)}\" hs=\"{SecurityElement.Escape(match.HomeSimplifiedChinese)}\" ae=\"{SecurityElement.Escape(match.AwayEnglish)}\" at=\"{SecurityElement.Escape(match.AwayTraditionalChinese)}\" as=\"{SecurityElement.Escape(match.AwaySimplifiedChinese)}\" mt=\"{match.KickOfTime}\" running=\"{match.IsLive}\"/>");
            }

            oddsFeedNode.Append("</league>");
        }

        private static void AppendEventNode(StringBuilder oddsFeedNode, Outright outright)
        {
            oddsFeedNode.Append($"<event id=\"{outright.EventId}\" ge=\"{SecurityElement.Escape(outright.EventEnglish)}\" gt=\"{SecurityElement.Escape(outright.EventTraditionalChinese)}\" gs=\"{SecurityElement.Escape(outright.EventSimplifiedChinese)}\" mt=\"{outright.Time}\" sport=\"{SecurityElement.Escape(outright.SportName)}\">");

            foreach (var outrightOdds in outright.OutrightOddsList)
            {
                oddsFeedNode.Append($"<team te=\"{SecurityElement.Escape(outrightOdds.OddNameEnglish)}\" tt=\"{SecurityElement.Escape(outrightOdds.OddNameTraditionalChinese)}\" ts=\"{SecurityElement.Escape(outrightOdds.OddNameSimplifiedChinese)}\" />");
            }

            oddsFeedNode.Append("</event>");
        }
    }
}