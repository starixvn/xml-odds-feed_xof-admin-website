namespace Fanex.OddsFeed.WebServiceLibrary.Filters
{
    using Common.DTO;

    public interface IMatchFilter
    {
        bool FilterMatch(Match match, UserProfile userProfile, UserQuery userQuery);

        bool FilterBetType(UserProfile userProfile, UserQuery userQuery, bool isSample, bool changeTime, int timeStamp, MatchOdds matchOdds);
    }
}
