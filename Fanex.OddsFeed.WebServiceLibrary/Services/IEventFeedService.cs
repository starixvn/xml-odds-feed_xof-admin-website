namespace Fanex.OddsFeed.WebServiceLibrary.Services
{
    using System.Xml;
    using Common.DTO;

    public interface IEventFeedService
    {
        XmlElement BuildXmlEvent(string timestamp, UserQuery userQuery, bool isSample, bool isOutright);
    }
}
