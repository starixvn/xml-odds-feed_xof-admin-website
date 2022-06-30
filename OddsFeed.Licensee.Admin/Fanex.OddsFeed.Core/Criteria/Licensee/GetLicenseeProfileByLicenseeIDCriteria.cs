namespace Fanex.OddsFeed.Core.Criteria.Licensee
{
    using Data.Repository;

    public class GetLicenseeProfileByLicenseeIDCriteria : CriteriaBase
    {
        public short SystemID { get; set; }

        public override string GetSettingKey() => "Odds_LicenseeProfileByLicenseeIDSel";
    }
}