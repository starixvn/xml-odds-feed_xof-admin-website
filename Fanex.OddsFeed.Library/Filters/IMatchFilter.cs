namespace Fanex.OddsFeed.Library.Filters
{
    using System.Collections.Generic;
    using Fanex.OddsFeed.Library.Nodes;

    public interface IMatchFilter
    {
        IList<Match> QueryMatches(OddsProfile oddsProfile, OddsFeedFilter oddsFeedFilter, IList<Match> matches, bool changeTime, int timeStamp);

        bool FilterBetType(OddsProfile oddsProfile, OddsFeedFilter oddsFeedFilter, bool isSample, MatchOdds matchOdds);
    }
}
