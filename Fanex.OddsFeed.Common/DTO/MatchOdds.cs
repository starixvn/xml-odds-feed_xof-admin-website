namespace Fanex.OddsFeed.Common.DTO
{
    using global::System;
    using global::System.Collections.Generic;

    [Serializable]
    public class MatchOdds
    {
        public int BetTypeId { get; set; }

        public string Tag { get; set; }

        public int TimeStamp { get; set; }

        public DateTime Time { get; set; }

        public IEnumerable<BetChoice> BetChoices { get; set; }
    }
}
