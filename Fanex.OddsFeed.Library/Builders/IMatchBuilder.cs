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
            IEnumerable<MatchNode> matches,
            string bookieName,
            DateTime lastUpdate,
            int timeStamp,
            string oddsType);
    }
}
