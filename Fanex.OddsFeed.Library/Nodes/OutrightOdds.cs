namespace Fanex.OddsFeed.Library.Nodes
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class OutrightOdds
    {
        public int OddsId { get; set; }

        public IEnumerable<Language> Names { get; set; }

        public decimal Value { get; set; }
    }
}
