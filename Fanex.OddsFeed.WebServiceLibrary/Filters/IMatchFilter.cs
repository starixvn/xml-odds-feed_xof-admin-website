namespace Fanex.OddsFeed.WebServiceLibrary.Filters
{
    using System.Collections.Generic;
    using Common.DTO;

    public interface IMatchFilter
    {
        IList<Match> QueryMatches(IList<Match> matches, UserProfile userProfile, UserQuery userQuery, bool changeTime, int timeStamp);

        bool FilterBetType(UserProfile userProfile, UserQuery userQuery, bool isSample, MatchOdds matchOdds);
    }
}
