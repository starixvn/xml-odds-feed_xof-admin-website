using Couchbase.Core;
using Fanex.OddsFeed.Common.DTO;

namespace Fanex.OddsFeed.WebServiceLibrary.Caching
{
    public interface IMatchCaching
    {
        SafeList<Match> GetMatches(ICluster cluster, UserProfile userProfile, UserQuery userQuery);
    }
}
