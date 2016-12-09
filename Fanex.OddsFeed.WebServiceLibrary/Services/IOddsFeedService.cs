namespace Fanex.OddsFeed.WebServiceLibrary.Services
{
    using System.Text;
    using Models;

    public interface IOddsFeedService
    {
        StringBuilder BuildXmlDocument(OddsFeedsModel model);
    }
}
