namespace Fanex.OddsFeed.Library.Nodes
{
    using System.Collections.Generic;

    public class EventOddsNode
    {
        public int OddsId { get; set; }

        public IEnumerable<Language> Names { get; set; }

        public string Value { get; set; }
    }
}
