namespace Fanex.OddsFeed.WebServiceLibrary.Services
{
    using System.Xml;

    public interface IEventFeedService
    {
        XmlElement BuildXmlEvent(string timestamp, OddsFeedFilter filter, bool isSample, bool isOutright);
    }
}
