namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using Data.Repository;

    public class GetBettypeByAffiliateIDCriteria : CriteriaBase
    {
        public int PartnerID { get; set; }

        public override string GetSettingKey() => "Odds_BettypeSelByPartnerId";

        public override bool IsValid() => true;
    }
}