namespace Fanex.OddsFeed.Core.Criteria.Licensee
{
    using Data.Repository;
    using Fanex.Data;

    public class EditLicenseeProfileCriteria : BaseCommand
    {
        public short SystemID { get; set; }

        public bool Editable { get; set; } = true;

        public string OddsType { get; set; } = string.Empty;

        public string LanguageList { get; set; } = string.Empty;

        public string OddsRegion { get; set; } = string.Empty;

        public string SportIDList { get; set; } = string.Empty;

        public string BetTypeIDList { get; set; } = string.Empty;

        public string LeagueIDList { get; set; } = string.Empty;

        public string CountryCodeList { get; set; } = string.Empty;

        public string LeagueKeyword { get; set; } = string.Empty;

        public string LeagueKeyword_O { get; set; } = string.Empty;

        public string OddsDelayTimes => string.Join(",", this.DeadballDelayTimes, this.LiveDelayTimes, this.OutrightDelayTimes);
        public bool OddsDelayTimesFlag { get; set; } = true;

        public string OutRightIDList { get; set; } = string.Empty;

        public short NumberOfIPs { get; set; } = 1;

        public bool OddsTypeFlag { get; set; } = true;

        public bool LanguageListFlag { get; set; } = true;

        public bool OddsRegionFlag { get; set; } = true;

        public bool SportIDListFlag { get; set; } = true;

        public bool BettypeIDListFlag { get; set; } = true;

        public bool LeagueIDListFlag { get; set; } = true;

        public bool CountryCodeListFlag { get; set; } = true;

        public bool LeagueKeywordFlag { get; set; } = true;

        public bool LeagueKeyword_OFlag { get; set; } = true;

        public int Creator { get; set; }

        public string IP { get; set; }

        public bool ApplyAllCurrentAffiliates { get; set; }

        public bool IsForceUpdate { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        // Ignored
        [SpParam(Ignored = true)]
        public string LeagueIDOutRightIDList => string.Join(";", this.LeagueIDList, this.OutRightIDList);

        [SpParam(Ignored = true)]
        public string DeadballDelayTimes { get; set; }

        [SpParam(Ignored = true)]
        public string LiveDelayTimes { get; set; }

        [SpParam(Ignored = true)]
        public string OutrightDelayTimes { get; set; }

        [SpParam(Ignored = true)]
        public string SystemName { get; set; }

        public string ResultDelayTimes { get; set; }
        public bool ResultDelayTimesFlag { get; set; } = true;

        public override string GetSettingKey() => "Odds_LicenseeProfileUpd";

        public override bool IsValid() => true;
    }
}