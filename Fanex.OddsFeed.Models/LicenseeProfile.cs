namespace Fanex.OddsFeed.Models
{
    public class LicenseeProfile
    {
        public string OddsType { get; set; }
        public string OddsRegion { get; set; }
        public string LanguageList { get; set; }

        public string SportIdList { get; set; }
        public string BetTypeIdList { get; set; }
        public string LeagueIdList { get; set; }
        public string CountryCodeList { get; set; }
        public string OutrightIdList { get; set; }

        public short NumberOfIPs { get; set; }
        public short ActualNumberOfIPs { get; set; }
        public string OddsDelayTimes { get; set; }
        public string ResultDelayTimes { get; set; }

        public string LeagueKeyword { get; set; }
        public string LeagueKeyword_O { get; set; }

        public bool OddsTypeFlag { get; set; }
        public bool LanguageListFlag { get; set; }
        public bool OddsRegionFlag { get; set; }
        public bool SportIdListFlag { get; set; }
        public bool BetTypeIdListFlag { get; set; }
        public bool LeagueIdListFlag { get; set; }
        public bool CountryCodeListFlag { get; set; }
        public bool LeagueKeywordFlag { get; set; }
        public bool LeagueKeyword_OFlag { get; set; }
        public bool OddsDelayTimesFlag { get; set; }
        public bool ResultDelayTimesFlag { get; set; }

        public bool Editable { get; set; }
    }
}