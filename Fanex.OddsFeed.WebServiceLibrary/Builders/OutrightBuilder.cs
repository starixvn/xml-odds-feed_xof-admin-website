namespace Fanex.OddsFeed.WebServiceLibrary.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using Common.Converters;
    using Common.DTO;
    using Filters;

    public class OutrightBuilder : OddsFeedBuilder, IOutrightBuilder
    {
        private OutrightFilter _outrightFilter;

        public OutrightBuilder()
        {
            _outrightFilter = new OutrightFilter();
        }

        public XmlNode CreateOutrightsNode(
            XmlDocument xmlDocument,
            UserProfile userProfile,
            UserQuery userQuery,
            IEnumerable<Outright> outrightNodes,
            bool isOutright,
            bool isSample)
        {
            var outrightsNode = xmlDocument.CreateNode(XmlNodeType.Element, "outrights", null);

            if (IsSampleOutright(isOutright, isSample, outrightNodes.Count()))
            {
                outrightsNode.AppendChild(CreateEventNode(xmlDocument, userProfile, outrightNodes.ElementAt(0)));
            }

            if (FilterBetType(userQuery, isSample))
            {
                foreach (var outrightNode in outrightNodes)
                {
                    outrightsNode.AppendChild(CreateEventNode(xmlDocument, userProfile, outrightNode));
                }
            }

            return outrightsNode;
        }

        private static bool IsSampleOutright(bool isOutright, bool isSample, int numberOutright)
        {
            return isSample && isOutright && numberOutright > 0;
        }

        public static bool FilterBetType(UserQuery userQuery, bool isSample)
        {
            return !isSample 
                && (userQuery.BetTypes == null
                || userQuery.BetTypes.Count == 0
                || (userQuery.BetTypes != null
                && userQuery.BetTypes.Contains(10)));
        }

        private XmlNode CreateEventNode(XmlDocument xmlDocument, UserProfile userProfile, Outright outrightNode)
        {
            var eventNode = xmlDocument.CreateNode(XmlNodeType.Element, "event", null);

            AssignEventId(xmlDocument, eventNode, outrightNode.EventId);
            AssignNames(xmlDocument, eventNode, outrightNode.EventNames, _leagueAttributes);
            AssignEventTime(xmlDocument, eventNode, outrightNode.Time);
            AssignSportName(xmlDocument, eventNode, outrightNode.SportName);

            foreach (var outrightOddsNode in outrightNode.OutrightOddsList)
            {
                eventNode.AppendChild(CreateOddNode(xmlDocument, userProfile, outrightOddsNode));
            }

            return eventNode;
        }

        private static void AssignEventId(XmlDocument xmlDocument, XmlNode eventNode, int eventId)
        {
            var idAttribute = xmlDocument.CreateAttribute("id");
            idAttribute.Value = eventId.ToString();
            eventNode.Attributes.Append(idAttribute);
        }

        private static void AssignEventTime(XmlDocument xmlDocument, XmlNode eventNode, DateTime eventTime)
        {
            var eventTimeAttribute = xmlDocument.CreateAttribute("mt");
            eventTimeAttribute.Value = eventTime.ToString();
            eventNode.Attributes.Append(eventTimeAttribute);
        }

        private static void AssignSportName(XmlDocument xmlDocument, XmlNode eventNode, string sportName)
        {
            var sportAttribute = xmlDocument.CreateAttribute("sport");
            sportAttribute.Value = sportName;
            eventNode.Attributes.Append(sportAttribute);
        }

        private XmlNode CreateOddNode(
            XmlDocument xmlDocument,
            UserProfile userProfile,
            OutrightOdds outrightOddsNode)
        {
            var oddNode = xmlDocument.CreateNode(XmlNodeType.Element, "odd", null);

            AssignOddId(xmlDocument, oddNode, outrightOddsNode.OddsId);
            AssignNames(xmlDocument, oddNode, outrightOddsNode.Names, _teamAttributes);
            AssignOddValue(xmlDocument, userProfile, oddNode, outrightOddsNode.Value);

            return oddNode;
        }

        private static void AssignOddId(XmlDocument xmlDocument, XmlNode eventNode, int oddId)
        {
            var idAttribute = xmlDocument.CreateAttribute("id");
            idAttribute.Value = oddId.ToString();
            eventNode.Attributes.Append(idAttribute);
        }

        private static void AssignOddValue(XmlDocument xmlDocument, UserProfile userProfile, XmlNode eventNode, decimal oddValue)
        {
            var valueAttribute = xmlDocument.CreateAttribute("o");
            valueAttribute.Value = OddsConverter.ConvertOdds(userProfile.OddRegion, oddValue).ToString();
            eventNode.Attributes.Append(valueAttribute);
        }
    }
}
