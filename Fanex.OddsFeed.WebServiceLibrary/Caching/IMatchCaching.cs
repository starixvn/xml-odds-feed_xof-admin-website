using Couchbase.Core;
using Fanex.OddsFeed.Common.DTO;
using Fanex.OddsFeed.WebServiceLibrary.Models;

namespace Fanex.OddsFeed.WebServiceLibrary.Caching
{
    public interface IMatchCaching
    {
        SafeList<Match> GetMatches(ICluster cluster, OddsFeedsModel model, CacheVersion cacheVersion);
    }
}
