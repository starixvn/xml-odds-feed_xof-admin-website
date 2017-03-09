using Fanex.OddsFeed.Common.DTO;

namespace Fanex.OddsFeed.WebServiceLibrary.Caching
{
    public interface IMatchCaching
    {
        SafeList<Match> GetMatches(UserProfile userProfile, UserQuery userQuery, bool isLive);
    }
}
