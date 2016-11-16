namespace Fanex.OddsFeed.Library.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using Filters;
    using Nodes;

    public class OutrightBuilder : OddsFeedBuilder, IOutrightBuilder
    {
        private OutrightFilter _outrightFilter;

        public OutrightBuilder()
        {
            _outrightFilter = new OutrightFilter();
        }

        public XmlNode CreateOutrightsNode(
            XmlDocument xmlDocument,
            OddsProfile oddsProfile,
            OddsFeedFilter oddsFeedFilter,
            IEnumerable<Outright> outrightNodes,
            bool isOutright,
            bool isSample)
        {
            var outrightsNode = xmlDocument.CreateNode(XmlNodeType.Element, "outrights", null);

            if (IsSampleOutright(isOutright, isSample, outrightNodes.Count()))
            {
                outrightsNode.AppendChild(CreateEventNode(xmlDocument, oddsProfile, outrightNodes.ElementAt(0)));
            }

            if (FilterBetType(oddsFeedFilter, isSample))
            {
                foreach (var outrightNode in outrightNodes)
                {
                    outrightsNode.AppendChild(CreateEventNode(xmlDocument, oddsProfile, outrightNode));
                }
            }

            return outrightsNode;
        }

        private bool IsSampleOutright(bool isOutright, bool isSample, int numberOutright)
        {
            return isSample && isOutright && numberOutright > 0;
        }

        public bool FilterBetType(OddsFeedFilter oddsFeedFilter, bool isSample)
        {
            return !isSample 
                && (oddsFeedFilter.BetTypes == null
                || oddsFeedFilter.BetTypes.Count == 0
                || (oddsFeedFilter.BetTypes != null
                && oddsFeedFilter.BetTypes.Contains(10)));
        }

        private XmlNode CreateEventNode(XmlDocument xmlDocument, OddsProfile oddsProfile, Outright outrightNode)
        {
            var eventNode = xmlDocument.CreateNode(XmlNodeType.Element, "event", null);

            AssignEventId(xmlDocument, eventNode, outrightNode.EventId);
            AssignNames(xmlDocument, eventNode, outrightNode.EventNames, _leagueAttributes);
            AssignEventTime(xmlDocument, eventNode, outrightNode.Time);
            AssignSportName(xmlDocument, eventNode, outrightNode.SportName);

            foreach (var outrightOddsNode in outrightNode.OutrightOddsList)
            {
                eventNode.AppendChild(CreateOddNode(xmlDocument, oddsProfile, outrightOddsNode));
            }

            return eventNode;
        }

        private void AssignEventId(XmlDocument xmlDocument, XmlNode eventNode, int eventId)
        {
            var idAttribute = xmlDocument.CreateAttribute("id");
            idAttribute.Value = eventId.ToString();
            eventNode.Attributes.Append(idAttribute);
        }

        private void AssignEventTime(XmlDocument xmlDocument, XmlNode eventNode, DateTime eventTime)
        {
            var eventTimeAttribute = xmlDocument.CreateAttribute("mt");
            eventTimeAttribute.Value = eventTime.ToString();
            eventNode.Attributes.Append(eventTimeAttribute);
        }

        private void AssignSportName(XmlDocument xmlDocument, XmlNode eventNode, string sportName)
        {
            var sportAttribute = xmlDocument.CreateAttribute("sport");
            sportAttribute.Value = sportName;
            eventNode.Attributes.Append(sportAttribute);
        }

        private XmlNode CreateOddNode(
            XmlDocument xmlDocument,
            OddsProfile oddsProfile,
            OutrightOdds outrightOddsNode)
        {
            var oddNode = xmlDocument.CreateNode(XmlNodeType.Element, "odd", null);

            AssignOddId(xmlDocument, oddNode, outrightOddsNode.OddsId);
            AssignNames(xmlDocument, oddNode, outrightOddsNode.Names, _teamAttributes);
            AssignOddValue(xmlDocument, oddsProfile, oddNode, outrightOddsNode.Value);

            return oddNode;
        }

        private void AssignOddId(XmlDocument xmlDocument, XmlNode eventNode, int oddId)
        {
            var idAttribute = xmlDocument.CreateAttribute("id");
            idAttribute.Value = oddId.ToString();
            eventNode.Attributes.Append(idAttribute);
        }

        private void AssignOddValue(XmlDocument xmlDocument, OddsProfile oddsProfile, XmlNode eventNode, decimal oddValue)
        {
            var valueAttribute = xmlDocument.CreateAttribute("o");
            valueAttribute.Value = Utilities.ConvertOdds(oddsProfile.OddRegion, oddValue).ToString();
            eventNode.Attributes.Append(valueAttribute);
        }
    }
}
