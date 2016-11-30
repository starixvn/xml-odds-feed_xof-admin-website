namespace Fanex.OddsFeed.WebServiceLibrary.Builders
{
    using System.Collections.Generic;
    using System.Xml;
    using Common.DTO;

    public interface IEventBuilder
    {
        XmlNode CreateOutrightsNode(XmlDocument xmlDocument, IEnumerable<Outright> outrights);

        XmlNode CreateMatchesNode(XmlDocument xmlDocument, IEnumerable<Match> matches);
    }
}
