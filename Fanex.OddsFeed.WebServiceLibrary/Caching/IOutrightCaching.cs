using Couchbase.Core;
using Fanex.OddsFeed.Common.DTO;
using Fanex.OddsFeed.WebServiceLibrary.Models;

namespace Fanex.OddsFeed.WebServiceLibrary.Caching
{
    public interface IOutrightCaching
    {
        SafeList<Outright> GetOutrights(ICluster cluster, OddsFeedsModel model, int cacheVersion);
    }
}
