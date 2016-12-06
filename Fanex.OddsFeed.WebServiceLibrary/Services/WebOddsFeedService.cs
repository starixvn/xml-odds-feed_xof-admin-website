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
    using Filters;

    public class WebOddsFeedService : IWebOddsFeedService
    {
        private UserProfile _userProfile;
        private string _bookieName;
        private string _domainName;
        private string _ipAddress;
        private string _timeStamp;

        private IMatchBuilder _matchBuilder;
        private IOutrightBuilder _outrightBuilder;

        private IMatchFilter _matchFilter;
        private IOutrightFilter _outrightFilter;

        public WebOddsFeedService(
            UserProfile userProfile, 
            string bookieName, 
            string domainName, 
            string ipAddress, 
            string timeStamp)
        {
            _userProfile = userProfile;
            _bookieName = bookieName;
            _domainName = domainName;
            _ipAddress = ipAddress;
            _timeStamp = timeStamp;

            _matchBuilder = new MatchBuilder();
            _outrightBuilder = new OutrightBuilder();

            _matchFilter = new MatchFilter();
            _outrightFilter = new OutrightFilter();
        }

        public StringBuilder BuildXmlDocument(
            UserQuery userQuery, 
            bool isSample = false, 
            bool isOutright = true)
        {
            var oddsFeedNode = new StringBuilder();
            var oddsData = OddFeedDataCachingOperator.GetOddsData(_userProfile.DelayTimes);

            AppendBeginOddsFeedNode(oddsFeedNode);

            if (IsShownOutright(isSample, isOutright))
            {
                AddOutrightNode(oddsFeedNode, userQuery, oddsData, isOutright, isSample);
            }

            AddMatchesNode(oddsFeedNode, userQuery, oddsData, isSample);

            AppendEndOddsFeedNode(oddsFeedNode);

            return oddsFeedNode;
        }

        private void AppendBeginOddsFeedNode(StringBuilder oddsFeedNode)
        {
            oddsFeedNode.Append("<oddsfeed>");
        }

        private void AppendEndOddsFeedNode(StringBuilder oddsFeedNode)
        {
            oddsFeedNode.Append("</oddsfeed>");
        }

        private static XmlDocument CreateXmlDocument()
        {
            var xmlDocument = new XmlDocument();
            var xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);

            xmlDocument.InsertBefore(xmlDeclaration, xmlDocument.DocumentElement);

            return xmlDocument;
        }

        private bool IsShownOutright(bool isSample, bool isOutrights)
        {
            return _userProfile.OddsType.Contains(8) && (!isSample || isOutrights);
        }

        private void AddOutrightNode(
            StringBuilder oddsFeedNode, 
            UserQuery userQuery,
            OddsData oddsData,
            bool isOutright, 
            bool isSample)
        {
            var outrights = _outrightFilter.QueryOutrights(_userProfile, userQuery, oddsData.Outrights);

            if (outrights != null)
            {
                _outrightBuilder.AppendOutrightsNode(oddsFeedNode, _userProfile, userQuery, outrights, isOutright, isSample);
            }
        }

        private void AddMatchesNode(
            StringBuilder oddsFeedNode,
            UserQuery userQuery,
            OddsData oddsData,
            bool isSample)
        {
            var changetime = !string.IsNullOrEmpty(_timeStamp);
            var timeStamp = changetime ? int.Parse(_timeStamp) : 0;

            var liveMatches = GetLiveMatches(userQuery, oddsData.LiveMatches, changetime, timeStamp);
            var deadballMatches = GetDeadMatches(userQuery, oddsData.DeadballMatches, changetime, timeStamp);

            if (liveMatches != null || deadballMatches != null)
            {
                _matchBuilder.AppendMatchesNode(
                    oddsFeedNode, 
                    _userProfile, 
                    userQuery, 
                    deadballMatches, 
                    liveMatches, 
                    _bookieName, 
                    GetLastUpdate(), 
                    isSample);
            }
        }

        private IList<Match> GetLiveMatches(UserQuery userQuery, IList<Match> liveMatches, bool changeTime, int timeStamp)
        {
            return HaveLiveBallMatches(liveMatches)
                ? _matchFilter.QueryMatches(_userProfile, userQuery, liveMatches, changeTime, timeStamp)
                : new List<Match>();
        }

        private bool HaveLiveBallMatches(IEnumerable<Match> liveMatches)
        {
            return _userProfile.OddsType.Contains(4) && liveMatches.Count() > 0;
        }

        private IList<Match> GetDeadMatches(UserQuery userQuery, IList<Match> deadMatches, bool changeTime, int timeStamp)
        {
            return HaveDeadBallMatches(deadMatches)
                ? _matchFilter.QueryMatches(_userProfile, userQuery, deadMatches, changeTime, timeStamp)
                : new List<Match>();
        }

        private bool HaveDeadBallMatches(IEnumerable<Match> deadMatches)
        {
            return _userProfile.OddsType.Contains(2) && deadMatches.Count() > 0;
        }

        private static DateTime GetLastUpdate()
        {
            return DateTime.Now.AddHours(12);
        }
    }
}
