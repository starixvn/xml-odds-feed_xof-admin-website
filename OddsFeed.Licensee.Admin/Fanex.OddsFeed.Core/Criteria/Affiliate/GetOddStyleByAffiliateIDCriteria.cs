namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using Data.Repository;

    public class GetOddStyleByAffiliateIDCriteria : CriteriaBase
    {
        public int PartnerID { get; set; }

        public override string GetSettingKey() => "Odds_OddsStyleSelByPartnerId";

        public override bool IsValid() => true;
    }
}