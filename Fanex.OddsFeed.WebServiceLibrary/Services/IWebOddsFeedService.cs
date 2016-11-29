namespace Fanex.OddsFeed.WebServiceLibrary.Services
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
