namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using Data.Repository;

    public class GetSportByAffiliateIDCriteria : CriteriaBase
    {
        public int PartnerID { get; set; }

        public override string GetSettingKey() => "Odds_SportSelByPartnerId";

        public override bool IsValid() => true;
    }
}