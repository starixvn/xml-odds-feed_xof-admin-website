namespace Fanex.OddsFeed.Criteria.Licensee
{
    using Data.Repository;

    public class GetLicenseeSettingByLicenseeIDCriteria : CriteriaBase
    {
        public short SystemID { get; set; }

        public override string GetSettingKey() => "Odds_LicenseeSettingByLicenseeIDSel";

        public override bool IsValid() => true;
    }
}