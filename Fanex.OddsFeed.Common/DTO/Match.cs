namespace Fanex.OddsFeed.Common.DTO
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    [Serializable]
    public class LeagueDocument
    {
        public int Version { get; set; }

        public int LeagueId { get; set; }

        public string LeagueEn { get; set; }

        public string LeagueTw { get; set; }

        public string LeagueCn { get; set; }

        public int SportId { get; set; }

        public string SportName { get; set; }

        public string CountryCode { get; set; }

        public IList<Match> Matches { get; set; }
    }

    [Serializable]
    public class Match
    {
        public int MatchId { get; set; }

        [JsonIgnore]
        public int LeagueId { get; set; }

        [JsonIgnore]
        public string LeagueEn { get; set; }

        [JsonIgnore]
        public string LeagueTw { get; set; }

        [JsonIgnore]
        public string LeagueCn { get; set; }

        public string HomeEn { get; set; }

        public string HomeTw { get; set; }

        public string HomeCn { get; set; }

        public string AwayEn { get; set; }

        public string AwayTw { get; set; }

        public string AwayCn { get; set; }

        [JsonIgnore]
        public int SportId { get; set; }

        [JsonIgnore]
        public string SportName { get; set; }

        [JsonIgnore]
        public string CountryCode { get; set; }

        public DateTime KickOfTime { get; set; }

        public int TimeStamp { get; set; }

        public IList<MatchOdds> MatchOddsList { get; set; }
    }
}
