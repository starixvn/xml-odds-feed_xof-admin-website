namespace Fanex.OddsFeed.Criteria.Affiliate
{
    using Fanex.Data;

    public class UpdateProfileCriteria : BaseNonQueryCommand
    {
        public int Creator { get; set; }

        public int PartnerID { get; set; }

        public string OddsType { get; set; }

        public string OddsRegion { get; set; }

        public string LanguageList { get; set; }

        public string SportIDList { get; set; }

        public string BetTypeIDList { get; set; }

        public string LeagueIDList { get; set; }

        public string CountryCodeList { get; set; }

        public string LeagueKeyword { get; set; }

        public string LeagueKeyword_O { get; set; }

        public string OddsDelayTimes => string.Join(",", this.DeadballDelayTimes, this.LiveDelayTimes, this.OutrightDelayTimes);

        public string OutRightIDList { get; set; }

        public short NumberOfIPs { get; set; }

        public bool OddsTypeFlag { get; set; }

        public bool LanguageListFlag { get; set; }

        public bool OddsRegionFlag { get; set; }

        public bool SportIDListFlag { get; set; }

        public bool BetTypeIDListFlag { get; set; }

        public bool CountryCodeListFlag { get; set; }

        // Ignored
        [SpParam(Ignored = true)]
        public string DeadballDelayTimes { get; set; }

        [SpParam(Ignored = true)]
        public string LiveDelayTimes { get; set; }

        [SpParam(Ignored = true)]
        public string OutrightDelayTimes { get; set; }

        public string ResultDelayTimes { get; set; }

        public override string GetSettingKey() => "Odds_Partner_Upd_Profile";

        public override bool IsValid() => true;
    }
}