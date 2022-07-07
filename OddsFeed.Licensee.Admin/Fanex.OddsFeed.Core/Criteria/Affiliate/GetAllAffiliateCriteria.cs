namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using Data.Repository;

    public class GetAllAffiliateCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Odds_Partner_GetAll";

        public override bool IsValid() => true;
    }
}