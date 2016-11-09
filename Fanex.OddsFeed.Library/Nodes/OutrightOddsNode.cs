namespace Fanex.OddsFeed.Library.Nodes
{
    using System.Collections.Generic;

    public class OutrightOddsNode
    {
        public int OddsId { get; set; }

        public IEnumerable<Language> Names { get; set; }

        public decimal Value { get; set; }
    }
}
