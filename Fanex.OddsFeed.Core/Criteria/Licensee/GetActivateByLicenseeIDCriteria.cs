namespace Fanex.OddsFeed.Core.Criteria.Licensee
{
    using Data.Repository;

    public class GetActivateByLicenseeIDCriteria : CriteriaBase
    {
        public int SystemID { get; set; }

        public override string GetSettingKey() => "Odds_Licensee_GetActivate";

        public override bool IsValid() => true;
    }
}