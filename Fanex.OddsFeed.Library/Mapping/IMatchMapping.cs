namespace Fanex.OddsFeed.Library.Mapping
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.OddsFeed.Library.Schemas;
    using Model;
    using Nodes;

    public interface IMatchMapping
    {
        IEnumerable<MatchNode> GetMatchNodes(
            DataTable dataTable,
            IEnumerable<BetTypeSchema> schemas,
            bool isLive,
            IEnumerable<OddFlex> oddsFlexs);
    }
}
