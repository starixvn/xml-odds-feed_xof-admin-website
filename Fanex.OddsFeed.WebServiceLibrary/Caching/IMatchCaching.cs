namespace Fanex.OddsFeed.WebServiceLibrary.Caching
{
    using Couchbase.Core;
    using Fanex.OddsFeed.Common.DTO;
    using Fanex.OddsFeed.WebServiceLibrary.Models;

    public interface IMatchCaching
    {
        SafeList<Match> GetMatches(ICluster cluster, OddsFeedsModel model, int cacheVersion);
    }
}
