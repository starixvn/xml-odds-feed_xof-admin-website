namespace Fanex.OddsFeed.Common.DTO
{
    using System.Collections.Generic;

    public class MatchDocument
    {
        public int Version { get; set; }

        public IList<Match> Matches { get; set; }
    }
}
