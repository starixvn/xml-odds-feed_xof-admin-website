namespace Fanex.OddsFeed.Library.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using Nodes;

    public interface IMatchBuilder
    {
        XmlNode CreateMatchesNode(
            XmlDocument xmlDocument,
            OddsProfile oddsProfile,
            OddsFeedFilter filter,
            IEnumerable<Match> deadMatches,
            IEnumerable<Match> liveMatches,
            string bookieName,
            DateTime lastUpdate,
            int timeStamp,
            bool isSample);
    }
}
