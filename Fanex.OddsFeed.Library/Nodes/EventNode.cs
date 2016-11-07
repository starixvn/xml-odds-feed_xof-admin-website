namespace Fanex.OddsFeed.Library.Nodes
{
    using System;
    using System.Collections.Generic;

    public class EventNode
    {
        public int EventId { get; set; }

        public IEnumerable<Language> EventNames { get; set; }

        public DateTime Time { get; set; }

        public int SportId { get; set; }

        public string SportName { get; set; }

        public IList<EventOddsNode> EventOddsNodes { get; set; }
    }
}
