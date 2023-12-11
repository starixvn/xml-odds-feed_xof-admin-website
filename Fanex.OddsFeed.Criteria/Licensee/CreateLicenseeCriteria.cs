namespace Fanex.OddsFeed.Criteria.Licensee
{
    using System.ComponentModel.DataAnnotations;
    using Fanex.Data;
    using Fanex.OddsFeed.Resources;

    public class CreateLicenseeCriteria : BaseNonQueryCommand
    {
        public short SystemID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        [RegularExpression(@"^[\w\s]{1,}$",
            ErrorMessageResourceName = "InvalidNameValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        public string SystemName { get; set; } = string.Empty;

        public int LicenseeID { get; set; }

        public bool IsActive { get; set; } = true;

        public string DomainList { get; set; } = string.Empty;

        public string ListIPFrom { get; set; } = string.Empty;

        public string ListIPTo { get; set; } = string.Empty;

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

        public string OutRightIDList { get; set; } = string.Empty;

        public int Creator { get; set; }

        public string IP { get; set; }

        [Required]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
            ErrorMessageResourceName = "EmailFormatValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        public string ContactEmail { get; set; } = string.Empty;

        // Ignored

        [SpParam(Ignored = true)]
        public string DeadballDelayTimes { get; set; }

        [SpParam(Ignored = true)]
        public string LiveDelayTimes { get; set; }

        [SpParam(Ignored = true)]
        public string OutrightDelayTimes { get; set; }

        public string ResultDelayTimes { get; set; }
        public bool ResultDelayTimesFlag { get; set; } = true;

        public override string GetSettingKey() => "Odds_LicenseeIns";

        public override bool IsValid() => true;
    }
}