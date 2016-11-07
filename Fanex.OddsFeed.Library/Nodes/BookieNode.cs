namespace Fanex.OddsFeed.Library.Nodes
{
    using System.Collections.Generic;

    public class BookieNode
    {
        public string EnglishName { get; set; }

        public string SimplifiedChineseName { get; set; }

        public string TraditionalChineseName { get; set; }

        public IEnumerable<MatchOddsNode> OddsNodes { get; set; }
    }
}
