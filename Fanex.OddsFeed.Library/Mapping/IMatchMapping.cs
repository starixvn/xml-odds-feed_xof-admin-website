namespace Fanex.OddsFeed.Library.Mapping
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.OddsFeed.Library.Schemas;
    using Model;
    using Nodes;

    public interface IMatchMapping
    {
        MatchOddsNode GetMatchOdds(IEnumerable<BetTypeSchema> schemas, DataRow row);

        MatchOddsNode GetMatchOddsByOddsFlex(IEnumerable<BetTypeSchema> schemas, DataRow row, IEnumerable<OddFlex> oddsFlexs);
    }
}
