namespace Fanex.OddsFeed.Common.DTO
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class Match
    {
        public int MatchId { get; set; }

        public int LeagueId { get; set; }

        public IEnumerable<Language> LeagueNames { get; set; }

        public IEnumerable<Language> HomeNames { get; set; }

        public IEnumerable<Language> AwayNames { get; set; }

        public int TimeStamp { get; set; }

        public int SportId { get; set; }

        public string SportName { get; set; }

        public string CountryCode { get; set; }

        public int BetTypeId { get; set; }

        public DateTime KickOfTime { get; set; }

        public bool IsRunning { get; set; }

        public bool IsLive { get; set; }

        public Bookie Bookie { get; set; }
    }
}
