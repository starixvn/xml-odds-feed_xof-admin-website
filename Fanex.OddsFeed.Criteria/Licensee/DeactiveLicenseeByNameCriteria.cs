namespace Fanex.OddsFeed.Criteria.Licensee
{
    using Fanex.Data.Repository;

    public class DeactiveLicenseeByNameCriteria : BaseNonQueryCommand
    {
        public string LicenseeName { get; set; }

        public override string GetSettingKey() => "Odds_DeactiveLicenseeAutomatically_OIC";

        public override bool IsValid() => !string.IsNullOrEmpty(LicenseeName);
    }
}
