namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using Data.Repository;

    public class GetAllCountryCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Odds_AllCountrySel";

        public override bool IsValid() => true;
    }
}