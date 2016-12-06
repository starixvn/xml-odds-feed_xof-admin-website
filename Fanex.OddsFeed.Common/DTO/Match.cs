namespace Fanex.OddsFeed.Common.DTO
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class Match
    {
        public int MatchId { get; set; }

        public int LeagueId { get; set; }

        public string LeagueEnglish { get; set; }

        public string LeagueTraditionalChinese { get; set; }

        public string LeagueSimplifiedChinese { get; set; }

        public string HomeEnglish { get; set; }

        public string HomeTraditionalChinese { get; set; }

        public string HomeSimplifiedChinese { get; set; }

        public string AwayEnglish { get; set; }

        public string AwayTraditionalChinese { get; set; }

        public string AwaySimplifiedChinese { get; set; }

        public int TimeStamp { get; set; }

        public int SportId { get; set; }

        public string SportName { get; set; }

        public string CountryCode { get; set; }

        public DateTime KickOfTime { get; set; }

        public bool IsRunning { get; set; }

        public bool IsLive { get; set; }

        public string BookieNameEnglish { get; set; }

        public string BookieNameTraditionalChinese { get; set; }

        public string BookieNameSimplifiedChinese { get; set; }

        public IList<MatchOdds> MatchOddsList { get; set; }
    }
}
