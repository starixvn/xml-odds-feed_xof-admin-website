using Fanex.OddsFeed.Common.DTO;

namespace Fanex.OddsFeed.WebServiceLibrary.Caching
{
    public class MatchCaching : IMatchCaching
    {
        public SafeList<Match> GetMatches(UserProfile userProfile, UserQuery userQuery, bool isLive)
        {
            return new SafeList<Match> { };
        }
    }
}
