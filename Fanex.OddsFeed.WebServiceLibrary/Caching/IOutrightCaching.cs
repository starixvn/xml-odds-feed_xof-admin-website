using Couchbase.Core;
using Fanex.OddsFeed.Common.DTO;

namespace Fanex.OddsFeed.WebServiceLibrary.Caching
{
    public interface IOutrightCaching
    {
        SafeList<Outright> GetOutrights(ICluster cluster, UserProfile userProfile, UserQuery userQuery);
    }
}
