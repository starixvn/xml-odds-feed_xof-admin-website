namespace Fanex.OddsFeed.Library.Builders
{
    using System.Collections.Generic;
    using System.Xml;
    using Nodes;

    public interface IEventBuilder
    {
        XmlNode CreateOutrightsNode(XmlDocument xmlDocument, IEnumerable<Outright> outrights);

        XmlNode CreateMatchesNode(XmlDocument xmlDocument, IEnumerable<Match> matches);
    }
}
