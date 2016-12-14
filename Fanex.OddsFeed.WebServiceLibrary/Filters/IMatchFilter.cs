namespace Fanex.OddsFeed.WebServiceLibrary.Filters
{
    using Common.DTO;

    public interface IMatchFilter
    {
        bool FilterMatch(Match match, UserProfile userProfile, UserQuery userQuery, bool changeTime, int timeStamp);

        bool FilterBetType(UserProfile userProfile, UserQuery userQuery, bool isSample, MatchOdds matchOdds);
    }
}
