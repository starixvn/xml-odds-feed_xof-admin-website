namespace Fanex.OddsFeed.Library.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using Filters;
    using Nodes;

    public class MatchBuilder : OddsFeedBuilder, IMatchBuilder
    {
        private MatchFilter _matchFilter;

        public MatchBuilder()
        {
            _matchFilter = new MatchFilter();
        }

        public XmlNode CreateMatchesNode(
            XmlDocument xmlDocument, 
            OddsProfile oddsProfile,
            OddsFeedFilter oddsFeedFilter,
            IEnumerable<Match> deadMatches,
            IEnumerable<Match> liveMatches, 
            string bookieName,
            DateTime lastUpdate,
            int timeStamp,
            bool isSample)
        {
            var matchesNode = xmlDocument.CreateNode(XmlNodeType.Element, "matches", null);

            AssignLastUpdate(xmlDocument, matchesNode, lastUpdate);
            AssignMatchTimeStamp(xmlDocument, matchesNode, timeStamp);
            AssignOddsType(xmlDocument, matchesNode, GetOddsTypeName(oddsProfile.OddRegion));

            if(deadMatches.Any())
            {
                AssignMatchNodes(xmlDocument, matchesNode, oddsProfile, oddsFeedFilter, deadMatches, bookieName, isSample);
            }
            
            if(liveMatches.Any())
            {
                AssignMatchNodes(xmlDocument, matchesNode, oddsProfile, oddsFeedFilter, liveMatches, bookieName, isSample);
            }

            return matchesNode;
        }

        private void AssignMatchNodes(
            XmlDocument xmlDocument, 
            XmlNode matchesNode, 
            OddsProfile oddsProfile,
            OddsFeedFilter oddsFeedFilter,
            IEnumerable<Match> matches,
            string bookieName,
            bool isSample)
        {
            if (isSample)
            {
                matchesNode.AppendChild(CreateMatchNode(xmlDocument, oddsProfile, oddsFeedFilter, matches.ElementAt(0), bookieName, isSample));
            }
            else
            {
                foreach (var match in matches)
                {
                    matchesNode.AppendChild(CreateMatchNode(xmlDocument, oddsProfile, oddsFeedFilter, match, bookieName, isSample));
                }
            }
        }

        private void AssignLastUpdate(XmlDocument xmlDocument, XmlNode matchesNode, DateTime lastUpdate)
        {
            var sportAttribute = xmlDocument.CreateAttribute("lastupdate");
            sportAttribute.Value = lastUpdate.ToString();
            matchesNode.Attributes.Append(sportAttribute);
        }

        private void AssignMatchTimeStamp(XmlDocument xmlDocument, XmlNode matchesNode, int timeStamp)
        {
            var sportAttribute = xmlDocument.CreateAttribute("timestamp");
            sportAttribute.Value = timeStamp.ToString();
            matchesNode.Attributes.Append(sportAttribute);
        }

        private void AssignOddsType(XmlDocument xmlDocument, XmlNode matchesNode, string oddsType)
        {
            var sportAttribute = xmlDocument.CreateAttribute("oddstype");
            sportAttribute.Value = oddsType;
            matchesNode.Attributes.Append(sportAttribute);
        }

        private XmlNode CreateMatchNode(
            XmlDocument xmlDocument, 
            OddsProfile oddsProfile, 
            OddsFeedFilter oddsFeedFilter, 
            Match match, 
            string bookieName,
            bool isSample)
        {
            var matchNode = xmlDocument.CreateNode(XmlNodeType.Element, "match", null);
            AssignMatchId(xmlDocument, matchNode, match.MatchId);
            AssignNames(xmlDocument, matchNode, match.LeagueNames, _leagueAttributes);
            AssignNames(xmlDocument, matchNode, match.HomeNames, _homeAttributes);
            AssignNames(xmlDocument, matchNode, match.AwayNames, _awayAttibutes);
            AssignMatchKickOff(xmlDocument, matchNode, match.KickOfTime);
            AssignSport(xmlDocument, matchNode, match.SportName);
            AssignMatchStatus(xmlDocument, matchNode, match.IsLive);

            matchNode.AppendChild(CreateBookieNode(xmlDocument, oddsProfile, oddsFeedFilter, match.Bookie, bookieName, isSample));

            return matchNode;
        }

        private void AssignMatchId(XmlDocument xmlDocument, XmlNode matchNode, int matchId)
        {
            var matchIdAttribute = xmlDocument.CreateAttribute("id");
            matchIdAttribute.Value = matchId.ToString();
            matchNode.Attributes.Append(matchIdAttribute);
        }

        private XmlNode CreateBookieNode(
            XmlDocument xmlDocument, 
            OddsProfile oddsProfile, 
            OddsFeedFilter oddsFeedFilter, 
            Bookie bookie, 
            string bookieName,
            bool isSample)
        {
            var bookieNode = xmlDocument.CreateNode(XmlNodeType.Element, "bookie", null);

            AssignBookieName(xmlDocument, bookieNode, bookieName);
            CrearteOddNodeList(xmlDocument, bookieNode, oddsProfile, oddsFeedFilter, bookie.MatchOddsList, isSample);

            return bookieNode;
        }

        private void AssignBookieName(XmlDocument xmlDocument, XmlNode bookieNode, string bookieName)
        {
            var nameAttribute = xmlDocument.CreateAttribute("name");
            nameAttribute.Value = bookieName.ToString();
            bookieNode.Attributes.Append(nameAttribute);

            var simpAttribute = xmlDocument.CreateAttribute("simp");
            simpAttribute.Value = bookieName.ToString();
            bookieNode.Attributes.Append(simpAttribute);

            var tradAttribute = xmlDocument.CreateAttribute("trad");
            tradAttribute.Value = bookieName.ToString();
            bookieNode.Attributes.Append(tradAttribute);
        }

        private void CrearteOddNodeList(
            XmlDocument xmlDocument, 
            XmlNode bookieNode, 
            OddsProfile oddsProfile, 
            OddsFeedFilter oddsFeedFilter,
            IEnumerable<MatchOdds> matchOddsNodes,
            bool isSample)
        {
            foreach (var matchOddsNode in matchOddsNodes)
            {
                if (_matchFilter.FilterBetType(oddsProfile, oddsFeedFilter, isSample, matchOddsNode))
                {
                    bookieNode.AppendChild(CreateOddNode(xmlDocument, oddsProfile, matchOddsNode));
                }
            }
        }

        private XmlNode CreateOddNode(XmlDocument xmlDocument, OddsProfile oddsProfile, MatchOdds matchOddsNode)
        {
            var oddXmlNode = xmlDocument.CreateNode(XmlNodeType.Element, "odd", null);

            AssignTag(xmlDocument, oddXmlNode, matchOddsNode.Tag);
            AssignBetChoices(xmlDocument, oddXmlNode, oddsProfile, matchOddsNode.BetChoices);
            AssignTime(xmlDocument, oddXmlNode, matchOddsNode.Time);
            AssignTimeStamp(xmlDocument, oddXmlNode, matchOddsNode.TimeStamp);

            return oddXmlNode;
        }

        private void AssignTag(XmlDocument xmlDocument, XmlNode oddsNode, string tag)
        {
            var attribute = xmlDocument.CreateAttribute("t");
            attribute.Value = tag;

            oddsNode.Attributes.Append(attribute);
        }

        private void AssignBetChoices(
            XmlDocument xmlDocument,
            XmlNode oddsNode,
            OddsProfile oddsProfile,
            IEnumerable<BetChoice> betChoices)
        {
            foreach (var betChoice in betChoices)
            {
                var attribute = xmlDocument.CreateAttribute(betChoice.Code);
                attribute.Value = ConvertOdds(oddsProfile, betChoice);
                oddsNode.Attributes.Append(attribute);
            }
        }

        private void AssignTime(XmlDocument xmlDocument, XmlNode oddsNode, DateTime time)
        {
            var attribute = xmlDocument.CreateAttribute("time");
            attribute.Value = time.ToString();

            oddsNode.Attributes.Append(attribute);
        }

        private void AssignTimeStamp(XmlDocument xmlDocument, XmlNode oddsNode, int timeStamp)
        {
            var attribute = xmlDocument.CreateAttribute("timestamp");
            attribute.Value = timeStamp.ToString();

            oddsNode.Attributes.Append(attribute);
        }

        private string ConvertOdds(OddsProfile oddsProfile, BetChoice betChoice)
        {
            if(betChoice.NotConvert)
            {
                return betChoice.Value;
            }

            return Utilities.ConvertOdds(oddsProfile.OddRegion, Convert.ToDecimal(betChoice.Value)).ToString();
        }

        private string GetOddsTypeName(int oddsTypeId)
        {
            string oddsTypeName = string.Empty;

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
