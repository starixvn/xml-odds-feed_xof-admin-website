namespace Fanex.OddsFeed.WebServiceLibrary.Services
{
    using System.Text;

    public interface IEventFeedService
    {
        StringBuilder BuildXmlEvent(bool isSample = false, bool isOutright = true);
    }
}
