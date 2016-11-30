namespace Fanex.OddsFeed.WebServiceLibrary.Builders
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using Common.DTO;

    public class EventBuilder : OddsFeedBuilder
    {
        public XmlNode CreateOutrightsNode(XmlDocument xmlDocument, IEnumerable<Outright> outrights)
        {
            var outrightsNode = xmlDocument.CreateNode(XmlNodeType.Element, "outrights", null);

            foreach (var outright in outrights)
            {
                outrightsNode.AppendChild(CreateEventNode(xmlDocument, outright));
            }

            return outrightsNode;
        }

        public XmlNode CreateMatchesNode(XmlDocument xmlDocument, IEnumerable<Match> matches)
        {
            var matchesNode = xmlDocument.CreateNode(XmlNodeType.Element, "matches", null);
            var leagues = matches.GroupBy(league => league.LeagueId);

            foreach (var league in leagues)
            {
                matchesNode.AppendChild(CreateLeagueNode(xmlDocument, league.Key, league.AsEnumerable()));
            }

            return matchesNode;
        }

        private XmlNode CreateLeagueNode(XmlDocument xmlDocument, int leagueId, IEnumerable<Match> matches)
        {
            var leagueNode = xmlDocument.CreateNode(XmlNodeType.Element, "league", null);

            AssignId(xmlDocument, leagueNode, leagueId);
            AssignNames(xmlDocument, leagueNode, matches.First().LeagueNames, _leagueAttributes);
            AssignSport(xmlDocument, leagueNode, matches.First().SportName);

            foreach (var match in matches)
            {
                leagueNode.AppendChild(CreateMatchNode(xmlDocument, match));
            }

            return leagueNode;
        }

        private XmlNode CreateMatchNode(XmlDocument xmlDocument, Match match)
        {
            var matchNode = xmlDocument.CreateNode(XmlNodeType.Element, "match", null);

            AssignId(xmlDocument, matchNode, match.MatchId);
            AssignNames(xmlDocument, matchNode, match.HomeNames, _homeAttributes);
            AssignNames(xmlDocument, matchNode, match.AwayNames, _awayAttibutes);
            AssignMatchKickOff(xmlDocument, matchNode, match.KickOfTime);
            AssignMatchStatus(xmlDocument, matchNode, match.IsLive);

            return matchNode;
        }

        private XmlNode CreateEventNode(XmlDocument xmlDocument, Outright outright)
        {
            var eventNode = xmlDocument.CreateNode(XmlNodeType.Element, "event", null);

            AssignId(xmlDocument, eventNode, outright.EventId);
            AssignNames(xmlDocument, eventNode, outright.EventNames, _leagueAttributes);
            AssignMatchKickOff(xmlDocument, eventNode, outright.Time);
            AssignSport(xmlDocument, eventNode, outright.SportName);

            foreach (var outrightOddsNode in outright.OutrightOddsList)
            {
                eventNode.AppendChild(CreateTeamNode(xmlDocument, outrightOddsNode));
            }

            return eventNode;
        }

        private XmlNode CreateTeamNode(XmlDocument xmlDocument, OutrightOdds outrightOddsNode)
        {
            var teamNode = xmlDocument.CreateNode(XmlNodeType.Element, "team", null);

            AssignNames(xmlDocument, teamNode, outrightOddsNode.Names, _teamAttributes);

            return teamNode;
        }
    }
}
