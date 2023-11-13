namespace Fanex.OddsFeed.Core.Criteria.Licensee
{
    using Data.Repository;

    public class GetLicenseeCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Odds_LicenseeSel";

        public override bool IsValid() => true;
    }
}