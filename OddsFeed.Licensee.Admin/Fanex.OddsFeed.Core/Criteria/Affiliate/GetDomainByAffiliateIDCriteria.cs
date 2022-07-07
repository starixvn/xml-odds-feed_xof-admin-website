namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using Data.Repository;

    public class GetDomainByAffiliateIDCriteria : CriteriaBase
    {
        public int PartnerID { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsOddsDomain { get; set; }

        public override string GetSettingKey() => "Odds_Domain_SelByPartnerId";

        public override bool IsValid() => true;
    }
}