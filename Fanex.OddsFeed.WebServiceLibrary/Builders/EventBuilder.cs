namespace Fanex.OddsFeed.WebServiceLibrary.Builders
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security;
    using System.Text;
    using Common.DTO;

    public class EventBuilder : IEventBuilder
    {
        public void AppendOutrightsNode(StringBuilder oddsFeedNode, UserLanguage userLanguage, IEnumerable<Outright> outrights)
        {
            oddsFeedNode.Append("<outrights>");

            foreach (var outright in outrights)
            {
                AppendEventNode(oddsFeedNode, userLanguage, outright);
            }

            oddsFeedNode.Append("</outrights>");
        }

        public void AppendMatchesNode(StringBuilder oddsFeedNode, UserLanguage userLanguage, IEnumerable<Match> matches)
        {
            var leagues = matches.GroupBy(league => league.LeagueId);

            oddsFeedNode.Append("<matches>");

            foreach (var league in leagues)
            {
                AppendLeagueNode(oddsFeedNode, userLanguage, league.Key, league.AsEnumerable());
            }

            oddsFeedNode.Append("</matches>");
        }

        private static void AppendLeagueNode(StringBuilder oddsFeedNode, UserLanguage userLanguage, int leagueId, IEnumerable<Match> matches)
        {
            var firstMatch = matches.First();

            AppendBeginLeague(oddsFeedNode, userLanguage, leagueId, firstMatch);

            foreach (var match in matches)
            {
                oddsFeedNode.Append($"<match id=\"{match.MatchId}\" ");

                AppendHomeTeam(oddsFeedNode, userLanguage, match);
                AppendAwayTeam(oddsFeedNode, userLanguage, match);

                oddsFeedNode.Append($"mt=\"{match.KickOfTime}\" running=\"{match.IsLive}\"/>");
            }

            oddsFeedNode.Append("</league>");
        }

        private static void AppendBeginLeague(StringBuilder oddsFeedNode, UserLanguage userLanguage, int leagueId, Match match)
        {
            oddsFeedNode.Append($"<league id=\"{leagueId}\" ");

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

            oddsFeedNode.Append($"sport=\"{SecurityElement.Escape(match.SportName)}\">");
        }

        private static void AppendHomeTeam(StringBuilder oddsFeedNode, UserLanguage userLanguage, Match match)
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

        private static void AppendAwayTeam(StringBuilder oddsFeedNode, UserLanguage userLanguage, Match match)
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

        private static void AppendEventNode(StringBuilder oddsFeedNode, UserLanguage userLanguage, Outright outright)
        {
            AppendBeginEvent(oddsFeedNode, userLanguage, outright);

            foreach (OutrightOdds outrightOdds in outright.OutrightOddsList)
            {
                AppendEventTeam(oddsFeedNode, userLanguage, outrightOdds);
            }

            oddsFeedNode.Append("</event>");
        }

        private static void AppendBeginEvent(StringBuilder oddsFeedNode, UserLanguage userLanguage, Outright outright)
        {
            oddsFeedNode.Append($"<event id=\"{outright.EventId}\" ");

            if (userLanguage.IsSupportedEnglish)
            {
                oddsFeedNode.Append($"ge=\"{SecurityElement.Escape(outright.EventEnglish)}\" ");
            }

            if (userLanguage.IsSupportedTraditionalChinese)
            {
                oddsFeedNode.Append($"gt=\"{SecurityElement.Escape(outright.EventTraditionalChinese)}\" ");
            }

            if (userLanguage.IsSupportedSimplifiedChinese)
            {
                oddsFeedNode.Append($"gs=\"{SecurityElement.Escape(outright.EventSimplifiedChinese)}\" ");
            }

            oddsFeedNode.Append($"mt=\"{outright.Time}\" sport=\"{SecurityElement.Escape(outright.SportName)}\">");
        }

        private static void AppendEventTeam(StringBuilder oddsFeedNode, UserLanguage userLanguage, OutrightOdds outrightOdds)
        {
            oddsFeedNode.Append($"<team ");

            if (userLanguage.IsSupportedEnglish)
            {
                oddsFeedNode.Append($"te=\"{SecurityElement.Escape(outrightOdds.OddNameEnglish)}\" ");
            }

            if (userLanguage.IsSupportedTraditionalChinese)
            {
                oddsFeedNode.Append($"tt=\"{SecurityElement.Escape(outrightOdds.OddNameTraditionalChinese)}\" ");
            }

            if (userLanguage.IsSupportedSimplifiedChinese)
            {
                oddsFeedNode.Append($"ts=\"{SecurityElement.Escape(outrightOdds.OddNameSimplifiedChinese)}\" ");
            }

            oddsFeedNode.Append($"/>");
        }
    }
}