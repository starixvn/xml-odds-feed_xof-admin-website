namespace Fanex.OddsFeed.Library.Nodes
{
    using System;
    using System.Collections.Generic;

    public class MatchNode
    {
        public int MatchId { get; set; }

        public IEnumerable<Language> Leagues { get; set; }

        public IEnumerable<Language> Homes { get; set; }

        public IEnumerable<Language> Aways { get; set; }

        public DateTime Time { get; set; }

        public string SportName { get; set; }

        public bool IsRunning { get; set; }

        public BookieNode BookieNode { get; set; }
    }
}
