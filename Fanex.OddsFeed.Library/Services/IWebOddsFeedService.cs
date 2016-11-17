namespace Fanex.OddsFeed.Library.Services
{
    using System.Xml;

    public interface IWebOddsFeedService
    {
        XmlElement BuildXmlDocument(
            string timeStamp,
            OddsFeedFilter oddsFeedFilter,
            bool isSample,
            bool isOutright);
    }
}
