namespace Fanex.OddsFeed.Core.Criteria.Licensee
{
    using Data.Repository;

    public class GetOddStyleByLicenseeIDCriteria : CriteriaBase
    {
        public int SystemID { get; set; }

        public override string GetSettingKey() => "Odds_OddsStyleSelBySystemId";

        public override bool IsValid() => true;
    }
}