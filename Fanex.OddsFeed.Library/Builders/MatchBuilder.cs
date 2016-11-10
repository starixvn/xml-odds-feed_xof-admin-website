namespace Fanex.OddsFeed.Library.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using Nodes;

    public class MatchBuilder : OddsFeedBuilder, IMatchBuilder
    {
        public XmlNode CreateMatchesNode(
            XmlDocument xmlDocument, 
            OddsProfile oddsProfile, 
            IEnumerable<MatchNode> matches, 
            string bookieName,
            DateTime lastUpdate,
            int timeStamp,
            string oddsType)
        {
            var matchNode = xmlDocument.CreateNode(XmlNodeType.Element, "matches", null);

            AssignLastUpdate(xmlDocument, matchNode, lastUpdate);
            AssignMatchTimeStamp(xmlDocument, matchNode, timeStamp);
            AssignOddsType(xmlDocument, matchNode, oddsType);

            foreach (var match in matches)
            {
                matchNode.AppendChild(CreateMatchNode(xmlDocument, oddsProfile, match, bookieName));
            }

            return matchNode;
        }

        private void AssignLastUpdate(XmlDocument xmlDocument, XmlNode matchNode, DateTime lastUpdate)
        {
            var sportAttribute = xmlDocument.CreateAttribute("lastupdate");
            sportAttribute.Value = lastUpdate.ToString();
            matchNode.Attributes.Append(sportAttribute);
        }

        private void AssignMatchTimeStamp(XmlDocument xmlDocument, XmlNode matchNode, int timeStamp)
        {
            var sportAttribute = xmlDocument.CreateAttribute("timestamp");
            sportAttribute.Value = timeStamp.ToString();
            matchNode.Attributes.Append(sportAttribute);
        }

        private void AssignOddsType(XmlDocument xmlDocument, XmlNode matchNode, string oddsType)
        {
            var sportAttribute = xmlDocument.CreateAttribute("oddstype");
            sportAttribute.Value = oddsType;
            matchNode.Attributes.Append(sportAttribute);
        }

        private XmlNode CreateMatchNode(XmlDocument xmlDocument, OddsProfile oddsProfile, MatchNode match, string bookieName)
        {
            var matchNode = xmlDocument.CreateNode(XmlNodeType.Element, "match", null);
            AssignMatchId(xmlDocument, matchNode, match.MatchId);
            AssignNames(xmlDocument, matchNode, match.LeagueNames, _leagueAttributes);
            AssignNames(xmlDocument, matchNode, match.HomeNames, _homeAttributes);
            AssignNames(xmlDocument, matchNode, match.AwayNames, _awayAttibutes);
            AssignMatchKickOff(xmlDocument, matchNode, match.KickOfTime);
            AssignSport(xmlDocument, matchNode, match.SportName);
            AssignMatchStatus(xmlDocument, matchNode, match.IsLive);

            matchNode.AppendChild(CreateBookieNode(xmlDocument, oddsProfile, match.BookieNode, bookieName));

            return matchNode;
        }

        private void AssignMatchId(XmlDocument xmlDocument, XmlNode matchNode, int matchId)
        {
            var matchIdAttribute = xmlDocument.CreateAttribute("id");
            matchIdAttribute.Value = matchId.ToString();
            matchNode.Attributes.Append(matchIdAttribute);
        }

        private void AssignMatchKickOff(XmlDocument xmlDocument, XmlNode matchNode, DateTime kickOffTime)
        {
            var mtAttribute = xmlDocument.CreateAttribute("mt");
            mtAttribute.Value = kickOffTime.ToString();
            matchNode.Attributes.Append(mtAttribute);
        }

        private void AssignSport(XmlDocument xmlDocument, XmlNode matchNode, string sportName)
        {
            var sportAttribute = xmlDocument.CreateAttribute("sport");
            sportAttribute.Value = sportName.ToString();
            matchNode.Attributes.Append(sportAttribute);
        }

        private void AssignMatchStatus(XmlDocument xmlDocument, XmlNode matchNode, bool isLive)
        {
            var runningAttribute = xmlDocument.CreateAttribute("running");
            runningAttribute.Value = isLive ? "1" : "0";
            matchNode.Attributes.Append(runningAttribute);
        }

        private XmlNode CreateBookieNode(XmlDocument xmlDocument, OddsProfile oddsProfile, BookieNode bookie, string bookieName)
        {
            var bookieNode = xmlDocument.CreateNode(XmlNodeType.Element, "bookie", null);

            AssignBookieName(xmlDocument, bookieNode, bookieName);
            CrearteOddNodeList(xmlDocument, bookieNode, oddsProfile, bookie.OddsNodes);

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
            IEnumerable<MatchOddsNode> matchOddsNodes)
        {
            foreach (var matchOddsNode in matchOddsNodes)
            {
                bookieNode.AppendChild(CreateOddNode(xmlDocument, oddsProfile, matchOddsNode));
            }
        }

        private XmlNode CreateOddNode(XmlDocument xmlDocument, OddsProfile oddsProfile, MatchOddsNode matchOddsNode)
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
    }
}
