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
           IEnumerable<OutrightNode> outrightNodes);
    }
}
