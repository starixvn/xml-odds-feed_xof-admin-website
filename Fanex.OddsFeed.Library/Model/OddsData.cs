namespace Fanex.OddsFeed.Library.Model
{
    using System.Collections.Generic;
    using Fanex.OddsFeed.Library.Nodes;

    public class OddsData
    {
        public IEnumerable<Match> LiveMatches { get; set; }

        public IEnumerable<Match> DeadballMatches { get; set; }

        public IEnumerable<Outright> Outrights { get; set; }
    }
}
