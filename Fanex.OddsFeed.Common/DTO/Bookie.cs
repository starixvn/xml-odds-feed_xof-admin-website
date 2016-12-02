namespace Fanex.OddsFeed.Common.DTO
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class Bookie
    {
        public string BookieNameEnglish { get; set; }

        public string BookieNameTraditionalChinese { get; set; }

        public string BookieNameSimplifiedChinese { get; set; }

        public IEnumerable<MatchOdds> MatchOddsList { get; set; }
    }
}
