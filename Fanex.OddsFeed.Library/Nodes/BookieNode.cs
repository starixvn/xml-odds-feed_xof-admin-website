namespace Fanex.OddsFeed.Library.Nodes
{
    using System.Collections.Generic;

    public class BookieNode
    {
        public IEnumerable<Language> BookieNames { get; set; }

        public IList<MatchOddsNode> OddsNodes { get; set; }
    }
}
