namespace Fanex.OddsFeed.WebServiceLibrary.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using Builders;
    using Caching;
    using Common.DTO;

    public class EventFeedService : IEventFeedService
    {
        private UserProfile _userProfile;
        private EventBuilder _eventBuilder;

        public EventFeedService(UserProfile userProfile)
        {
            _userProfile = userProfile;
            _eventBuilder = new EventBuilder();
        }

        public XmlElement BuildXmlEvent(string timestamp, UserQuery userQuery, bool isSample = false, bool isOutright = true)
        {
            var xmlDocument = CreateXmlDocument();
            var nodeRoot = xmlDocument.CreateElement("eventfeed");
            var oddsData = OddFeedDataCachingOperator.GetOddsData(_userProfile.DelayTimes);
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
