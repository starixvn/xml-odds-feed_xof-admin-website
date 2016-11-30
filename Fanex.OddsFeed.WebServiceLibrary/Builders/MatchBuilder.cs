namespace Fanex.OddsFeed.WebServiceLibrary.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using Common.Converters;
    using Common.DTO;
    using Filters;

    public class MatchBuilder : OddsFeedBuilder, IMatchBuilder
    {
        private MatchFilter _matchFilter;

        public MatchBuilder()
        {
            _matchFilter = new MatchFilter();
        }

        public XmlNode CreateMatchesNode(
            XmlDocument xmlDocument,
            UserProfile userProfile,
            UserQuery userQuery,
            IEnumerable<Match> deadballMatches,
            IEnumerable<Match> liveMatches, 
            string bookieName,
            DateTime lastUpdate,
            int timeStamp,
            bool isSample)
        {
            var matchesNode = xmlDocument.CreateNode(XmlNodeType.Element, "matches", null);

            AssignMatchesAttributes(xmlDocument, matchesNode, userProfile, deadballMatches, liveMatches, lastUpdate);

            if (deadballMatches.Any())
            {
                AssignMatchNodes(xmlDocument, matchesNode, userProfile, userQuery, deadballMatches, bookieName, isSample);
            }
            
            if(liveMatches.Any())
            {
                AssignMatchNodes(xmlDocument, matchesNode, userProfile, userQuery, liveMatches, bookieName, isSample);
            }

            return matchesNode;
        }

        private static void AssignMatchesAttributes(
            XmlDocument xmlDocument, 
            XmlNode matchesNode,
            UserProfile userProfile,
            IEnumerable<Match> deadballMatches,
            IEnumerable<Match> liveMatches,
            DateTime lastUpdate)
        {
            if (deadballMatches.Any() || liveMatches.Any())
            {
                var matchesTimeStamp = GetMatchesTimeStamp(deadballMatches, liveMatches);

                AssignLastUpdate(xmlDocument, matchesNode, lastUpdate);
                AssignMatchTimeStamp(xmlDocument, matchesNode, matchesTimeStamp);
                AssignOddsType(xmlDocument, matchesNode, GetOddsTypeName(userProfile.OddRegion));
            }
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

        private void AssignMatchNodes(
            XmlDocument xmlDocument, 
            XmlNode matchesNode,
            UserProfile userProfile,
            UserQuery userQuery,
            IEnumerable<Match> matches,
            string bookieName,
            bool isSample)
        {
            if (isSample)
            {
                matchesNode.AppendChild(CreateMatchNode(xmlDocument, userProfile, userQuery, matches.ElementAt(0), bookieName, isSample));
            }
            else
            {
                foreach (var match in matches)
                {
                    matchesNode.AppendChild(CreateMatchNode(xmlDocument, userProfile, userQuery, match, bookieName, isSample));
                }
            }
        }

        private static void AssignLastUpdate(XmlDocument xmlDocument, XmlNode matchesNode, DateTime lastUpdate)
        {
            var sportAttribute = xmlDocument.CreateAttribute("lastupdate");
            sportAttribute.Value = lastUpdate.ToString();
            matchesNode.Attributes.Append(sportAttribute);
        }

        private static void AssignMatchTimeStamp(XmlDocument xmlDocument, XmlNode matchesNode, int timeStamp)
        {
            var sportAttribute = xmlDocument.CreateAttribute("timestamp");
            sportAttribute.Value = timeStamp.ToString();
            matchesNode.Attributes.Append(sportAttribute);
        }

        private static void AssignOddsType(XmlDocument xmlDocument, XmlNode matchesNode, string oddsType)
        {
            var sportAttribute = xmlDocument.CreateAttribute("oddstype");
            sportAttribute.Value = oddsType;
            matchesNode.Attributes.Append(sportAttribute);
        }

        private XmlNode CreateMatchNode(
            XmlDocument xmlDocument,
            UserProfile userProfile,
            UserQuery userQuery, 
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

            matchNode.AppendChild(CreateBookieNode(xmlDocument, userProfile, userQuery, match.Bookie, bookieName, isSample));

            return matchNode;
        }

        private static void AssignMatchId(XmlDocument xmlDocument, XmlNode matchNode, int matchId)
        {
            var matchIdAttribute = xmlDocument.CreateAttribute("id");
            matchIdAttribute.Value = matchId.ToString();
            matchNode.Attributes.Append(matchIdAttribute);
        }

        private XmlNode CreateBookieNode(
            XmlDocument xmlDocument, 
            UserProfile userProfile, 
            UserQuery userQuery, 
            Bookie bookie, 
            string bookieName,
            bool isSample)
        {
            var bookieNode = xmlDocument.CreateNode(XmlNodeType.Element, "bookie", null);

            AssignBookieName(xmlDocument, bookieNode, bookieName);
            CrearteOddNodeList(xmlDocument, bookieNode, userProfile, userQuery, bookie.MatchOddsList, isSample);

            return bookieNode;
        }

        private static void AssignBookieName(XmlDocument xmlDocument, XmlNode bookieNode, string bookieName)
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
            UserProfile userProfile,
            UserQuery userQuery,
            IEnumerable<MatchOdds> matchOddsNodes,
            bool isSample)
        {
            foreach (var matchOddsNode in matchOddsNodes)
            {
                if (_matchFilter.FilterBetType(userProfile, userQuery, isSample, matchOddsNode))
                {
                    bookieNode.AppendChild(CreateOddNode(xmlDocument, userProfile, matchOddsNode));
                }
            }
        }

        private static XmlNode CreateOddNode(XmlDocument xmlDocument, UserProfile userProfile, MatchOdds matchOddsNode)
        {
            var oddXmlNode = xmlDocument.CreateNode(XmlNodeType.Element, "odd", null);

            AssignTag(xmlDocument, oddXmlNode, matchOddsNode.Tag);
            AssignBetChoices(xmlDocument, oddXmlNode, userProfile, matchOddsNode.BetChoices);
            AssignTime(xmlDocument, oddXmlNode, matchOddsNode.Time);
            AssignTimeStamp(xmlDocument, oddXmlNode, matchOddsNode.TimeStamp);

            return oddXmlNode;
        }

        private static void AssignTag(XmlDocument xmlDocument, XmlNode oddsNode, string tag)
        {
            var attribute = xmlDocument.CreateAttribute("t");
            attribute.Value = tag;

            oddsNode.Attributes.Append(attribute);
        }

        private static void AssignBetChoices(
            XmlDocument xmlDocument,
            XmlNode oddsNode,
            UserProfile userProfile,
            IEnumerable<BetChoice> betChoices)
        {
            foreach (var betChoice in betChoices)
            {
                var attribute = xmlDocument.CreateAttribute(betChoice.Code);
                attribute.Value = ConvertOdds(userProfile, betChoice);
                oddsNode.Attributes.Append(attribute);
            }
        }

        private static void AssignTime(XmlDocument xmlDocument, XmlNode oddsNode, DateTime time)
        {
            var attribute = xmlDocument.CreateAttribute("time");
            attribute.Value = time.ToString();

            oddsNode.Attributes.Append(attribute);
        }

        private static void AssignTimeStamp(XmlDocument xmlDocument, XmlNode oddsNode, int timeStamp)
        {
            var attribute = xmlDocument.CreateAttribute("timestamp");
            attribute.Value = timeStamp.ToString();

            oddsNode.Attributes.Append(attribute);
        }

        private static string ConvertOdds(UserProfile UserProfile, BetChoice betChoice)
        {
            if(betChoice.NotConvert)
            {
                return betChoice.Value;
            }

            return OddsConverter.ConvertOdds(UserProfile.OddRegion, Convert.ToDecimal(betChoice.Value)).ToString();
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
