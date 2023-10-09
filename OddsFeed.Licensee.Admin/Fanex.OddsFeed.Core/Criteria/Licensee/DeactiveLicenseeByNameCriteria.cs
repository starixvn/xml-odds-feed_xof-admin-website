namespace Fanex.OddsFeed.Core.Criteria.Licensee
{
    public class DeactiveLicenseeByNameCriteria : ResultCriteria
    {
        public string LicenseeName { get; set; }

        public override string GetSettingKey() => "Odds_DeactiveLicenseeAutomatically_OIC";

        public override bool IsValid() => !string.IsNullOrEmpty(LicenseeName);
    }
}
