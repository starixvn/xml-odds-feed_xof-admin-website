namespace Fanex.OddsFeed.Library.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using Builders;

    public class EventOddsFeedService : IEventOddsFeedService
    {
        private OddsProfile _oddsProfile;
        private EventBuilder _eventBuilder;

        public EventOddsFeedService(OddsProfile oddsProfile)
        {
            _oddsProfile = oddsProfile;
        }

        public XmlElement BuildXmlEvent(string timestamp, OddsFeedFilter filter, bool isSample = false, bool isOutright = true)
        {
            var xmlDocument = CreateXmlDocument();
            var nodeRoot = xmlDocument.CreateElement("eventfeed");
            var oddsData = Caching.GetOddsData(_oddsProfile.DelayTimes);
            var fullMatches = oddsData.DeadballMatches.Concat(oddsData.LiveMatches);

            if (IsShownOutRightEvent(isSample, isOutright))
            {
                nodeRoot.AppendChild(_eventBuilder.CreateOutrightsNode(xmlDocument, oddsData.Outrights));
            }

            nodeRoot.AppendChild(_eventBuilder.CreateMatchesNode(xmlDocument, fullMatches));

            return nodeRoot;
        }

        private XmlDocument CreateXmlDocument()
        {
            var xmlDocument = new XmlDocument();
            var xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);

            xmlDocument.InsertBefore(xmlDeclaration, xmlDocument.DocumentElement);

            return xmlDocument;
        }

        private bool IsShownOutRightEvent(bool isSample, bool isOutrights)
        {
            return !isSample || isOutrights;
        }
    }
}
