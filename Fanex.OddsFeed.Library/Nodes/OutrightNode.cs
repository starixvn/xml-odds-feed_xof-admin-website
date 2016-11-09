namespace Fanex.OddsFeed.Library.Nodes
{
    using System;
    using System.Collections.Generic;

    public class OutrightNode
    {
        public int EventId { get; set; }

        public IEnumerable<Language> EventNames { get; set; }

        public DateTime Time { get; set; }

        public int SportId { get; set; }

        public string SportName { get; set; }

        public IList<OutrightOddsNode> OutrightOddsNodes { get; set; }
    }
}
