namespace Fanex.OddsFeed.Library.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using Builders;
    using Filters;
    using Model;
    using Nodes;

    public class WebOddsFeedService
    {
        private OddsProfile _oddsProfile;
        private string _bookieName;
        private string _domainName;
        private string _ipAddress;
        private string _timeStamp;

        private IMatchBuilder _matchBuilder;
        private IOutrightBuilder _outrightBuilder;

        private MatchFilter _matchFilter;
        private OutrightFilter _outrightFilter;

        public WebOddsFeedService(
            OddsProfile oddsProfile, 
            string bookieName, 
            string domainName, 
            string ipAddress, 
            string timeStamp)
        {
            _oddsProfile = oddsProfile;
            _bookieName = bookieName;
            _domainName = domainName;
            _ipAddress = ipAddress;
            _timeStamp = timeStamp;

            _matchBuilder = new MatchBuilder();
            _outrightBuilder = new OutrightBuilder();

            _matchFilter = new MatchFilter();
            _outrightFilter = new OutrightFilter();
        }

        public XmlElement BuildXmlDocument(
            string timeStamp, 
            OddsFeedFilter oddsFeedFilter, 
            bool isSample = false, 
            bool isOutright = true)
        {
            var xmlDocument = CreateXmlDocument();
            var nodeRoot = xmlDocument.CreateElement("oddsfeed");
            var oddsData = Caching.GetOddsData(_oddsProfile.DelayTimes);

            if (IsShownOutright(isSample, isOutright))
            {
                AddOutrightNode(nodeRoot, xmlDocument, oddsFeedFilter, oddsData, isOutright, isSample);
            }

            AddMatchesNode(nodeRoot, xmlDocument, oddsFeedFilter, oddsData, isSample);

            return nodeRoot;
        }

        public XmlElement BuildXmlEvent(string timestamp, OddsFeedFilter filter, bool isSample = false, bool isOutrights = true)
        {
            var xmlDocument = CreateXmlDocument();
            var nodeRoot = xmlDocument.CreateElement("eventfeed");

            return nodeRoot;
        }

        private XmlDocument CreateXmlDocument()
        {
            var xmlDocument = new XmlDocument();
            var xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);

            xmlDocument.InsertBefore(xmlDeclaration, xmlDocument.DocumentElement);

            return xmlDocument;
        }

        private bool IsShownOutright(bool isSample, bool isOutrights)
        {
            return _oddsProfile.OddsType.Contains(8) && (!isSample || isOutrights);
        }

        private void AddOutrightNode(
            XmlElement nodeRoot, 
            XmlDocument xmlDocument, 
            OddsFeedFilter oddsFeedFilter,
            OddsData oddsData,
            bool isOutright, 
            bool isSample)
        {
            var outrights = _outrightFilter.QueryOutrights(_oddsProfile, oddsFeedFilter, oddsData.Outrights);

            if (outrights != null)
            {
                var outrightsNode = _outrightBuilder.CreateOutrightsNode(
                    xmlDocument, 
                    _oddsProfile, 
                    oddsFeedFilter, 
                    outrights, 
                    isOutright, 
                    isSample);

                nodeRoot.AppendChild(outrightsNode);
            }
        }

        private void AddMatchesNode(
            XmlElement nodeRoot,
            XmlDocument xmlDocument,
            OddsFeedFilter oddsFeedFilter,
            OddsData oddsData,
            bool isSample)
        {
            var changetime = !string.IsNullOrEmpty(_timeStamp);
            int timeStamp = changetime ? int.Parse(_timeStamp) : 0;

            var liveMatches = GetLiveMatches(oddsFeedFilter, oddsData.LiveMatches, changetime, timeStamp);
            var deadballMatches = GetDeadMatches(oddsFeedFilter, oddsData.DeadballMatches, changetime, timeStamp);

            if (liveMatches != null || deadballMatches != null)
            {
                var matchesNode = _matchBuilder.CreateMatchesNode(
                    xmlDocument, 
                    _oddsProfile,
                    oddsFeedFilter,
                    deadballMatches, 
                    liveMatches, 
                    _bookieName,
                    GetLastUpdate(), 
                    timeStamp,
                    isSample);

                nodeRoot.AppendChild(matchesNode);
            }
        }

        private IList<MatchNode> GetLiveMatches(OddsFeedFilter oddsFeedFilter, IEnumerable<MatchNode> liveMatches, bool changeTime, int timeStamp)
        {
            return HaveLiveBallMatches(liveMatches)
                ? _matchFilter.QueryMatches(_oddsProfile, oddsFeedFilter, liveMatches, changeTime, timeStamp)
                : new List<MatchNode>();
        }

        private bool HaveLiveBallMatches(IEnumerable<MatchNode> liveMatches)
        {
            return _oddsProfile.OddsType.Contains(4) && liveMatches.Count() > 0;
        }

        private IList<MatchNode> GetDeadMatches(OddsFeedFilter oddsFeedFilter, IEnumerable<MatchNode> deadMatches, bool changeTime, int timeStamp)
        {
            return HaveDeadBallMatches(deadMatches)
                ? _matchFilter.QueryMatches(_oddsProfile, oddsFeedFilter, deadMatches, changeTime, timeStamp)
                : new List<MatchNode>();
        }

        private bool HaveDeadBallMatches(IEnumerable<MatchNode> deadMatches)
        {
            return _oddsProfile.OddsType.Contains(2) && deadMatches.Count() > 0;
        }

        private DateTime GetLastUpdate()
        {
            return DateTime.Now.AddHours(12);
        }

        private int GetMaxTimeStamp(IEnumerable<MatchNode> deadBallMatches, IEnumerable<MatchNode> liveMatches)
        {
            int deadBallTimeStamp = 0;
            int liveTimeStamp = 0;

            if (deadBallMatches.Any())
            {
                deadBallTimeStamp = deadBallMatches.Max(match => match.TimeStamp);
            }

            if (liveMatches.Any())
            {
                liveTimeStamp = liveMatches.Max(match => match.TimeStamp);
            }

            return deadBallTimeStamp > liveTimeStamp ? deadBallTimeStamp : liveTimeStamp;
        }
    }
}
