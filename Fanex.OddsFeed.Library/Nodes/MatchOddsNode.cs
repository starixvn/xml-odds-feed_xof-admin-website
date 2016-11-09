namespace Fanex.OddsFeed.Library.Nodes
{
    using System;
    using System.Collections.Generic;

    public class MatchOddsNode
    {
        public int BetTypeId { get; set; }

        public string Tag { get; set; }

        public int TimeStamp { get; set; }

        public DateTime Time { get; set; }

        public IEnumerable<BetChoice> BetChoices { get; set; }
    }
}
