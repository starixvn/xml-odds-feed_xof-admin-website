namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using Data.Repository;

    public class GetCountryBySystemIDCriteria : CriteriaBase
    {
        public short SystemID { get; set; }

        public override string GetSettingKey() => "Odds_CountrySel";

        public override bool IsValid() => true;
    }
}