namespace Fanex.OddsFeed.Common.DTO
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class Bookie
    {
        public IEnumerable<Language> BookieNames { get; set; }

        public IList<MatchOdds> MatchOddsList { get; set; }
    }
}
