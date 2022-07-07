namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using Data.Repository;

    public class GetCountryByAffiliateIDCriteria : CriteriaBase
    {
        public int PartnerID { get; set; }

        public override string GetSettingKey() => "Odds_CountrySelByPartnerId";

        public override bool IsValid() => true;
    }
}