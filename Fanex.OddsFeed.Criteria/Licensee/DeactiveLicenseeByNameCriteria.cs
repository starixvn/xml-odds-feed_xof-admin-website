namespace Fanex.OddsFeed.Criteria.Licensee
{
    using Fanex.Data.Repository;

    public class DeactiveLicenseeByNameCriteria : CriteriaBase
    {
        public string LicenseeName { get; set; }
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; } = string.Empty;

        public override string GetSettingKey() => "Odds_DeactiveLicenseeAutomatically_OIC";

        public override bool IsValid() => !string.IsNullOrEmpty(LicenseeName);
    }
}
