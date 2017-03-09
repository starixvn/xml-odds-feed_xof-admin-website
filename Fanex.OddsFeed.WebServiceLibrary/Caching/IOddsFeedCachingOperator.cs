using Fanex.OddsFeed.Common.DTO;

namespace Fanex.OddsFeed.WebServiceLibrary.Caching
{
    public interface IOddsFeedCachingOperator
    {
        OddsData GetOddsData(UserProfile userProfile, UserQuery userQuery);
    }
}
