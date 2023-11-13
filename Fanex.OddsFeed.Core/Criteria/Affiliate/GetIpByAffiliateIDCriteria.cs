namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using Data.Repository;

    public class GetIpByAffiliateIDCriteria : CriteriaBase
    {
        public int PartnerID { get; set; }

        public int? IPFrom { get; set; }

        public override string GetSettingKey() => "Odds_Ip_SelByPartnerId";

        public override bool IsValid() => true;
    }
}