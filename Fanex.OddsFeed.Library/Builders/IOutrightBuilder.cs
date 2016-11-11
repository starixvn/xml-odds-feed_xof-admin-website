namespace Fanex.OddsFeed.Library.Builders
{
    using System.Collections.Generic;
    using System.Xml;
    using Nodes;

    public interface IOutrightBuilder
    {
        XmlNode CreateOutrightsNode(
            XmlDocument xmlDocument,
            OddsProfile oddsProfile,
            OddsFeedFilter oddsFeedFilter,
            IEnumerable<OutrightNode> outrightNodes,
            bool isOutright,
            bool isSample);
    }
}
