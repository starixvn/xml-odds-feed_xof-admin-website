namespace Fanex.OddsFeed.WebServiceLibrary.Services
{
    using System.Text;
    using Common.DTO;

    public interface IWebOddsFeedService
    {
        StringBuilder BuildXmlDocument(
            UserQuery userQuery,
            bool isSample,
            bool isOutright);
    }
}
