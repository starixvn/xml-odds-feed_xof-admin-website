namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using Data.Repository;

    public class GetAffiliateBySystemIDCriteria : CriteriaBase
    {
        public byte SystemId { get; set; }

        public override string GetSettingKey() => "Odds_Partner_GetBySytemId";

        public override bool IsValid() => true;
    }
}