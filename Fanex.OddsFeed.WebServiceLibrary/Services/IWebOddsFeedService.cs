namespace Fanex.OddsFeed.WebServiceLibrary.Services
{
    using System.Xml;
    using Common.DTO;

    public interface IWebOddsFeedService
    {
        XmlElement BuildXmlDocument(
            string timeStamp,
            UserQuery userQuery,
            bool isSample,
            bool isOutright);
    }
}
