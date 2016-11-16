namespace Fanex.OddsFeed.Library.Nodes
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class BookieNode
    {
        public IEnumerable<Language> BookieNames { get; set; }

        public IList<MatchOddsNode> OddsNodes { get; set; }
    }
}
