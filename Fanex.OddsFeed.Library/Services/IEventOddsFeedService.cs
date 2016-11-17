namespace Fanex.OddsFeed.Library.Services
{
    using System.Xml;

    public interface IEventOddsFeedService
    {
        XmlElement BuildXmlEvent(string timestamp, OddsFeedFilter filter, bool isSample, bool isOutright);
    }
}
