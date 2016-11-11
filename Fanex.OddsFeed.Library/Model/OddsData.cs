namespace Fanex.OddsFeed.Library.Model
{
    using System.Collections.Generic;
    using Fanex.OddsFeed.Library.Nodes;

    public class OddsData
    {
        public IEnumerable<MatchNode> LiveMatches { get; set; }

        public IEnumerable<MatchNode> DeadballMatches { get; set; }

        public IEnumerable<OutrightNode> Outrights { get; set; }
    }
}
