namespace Fanex.OddsFeed.Library.Model
{
    using System.Collections.Generic;
    using Fanex.OddsFeed.Library.Nodes;

    public class OddsData
    {
        public IList<Match> LiveMatches { get; set; }

        public IList<Match> DeadballMatches { get; set; }

        public IList<Outright> Outrights { get; set; }
    }
}
