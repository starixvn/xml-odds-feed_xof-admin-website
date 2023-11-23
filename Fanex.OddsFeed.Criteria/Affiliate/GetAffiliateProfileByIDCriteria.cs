namespace Fanex.OddsFeed.Criteria.Affiliate
{
    using Data.Repository;

    public class GetAffiliateProfileByIDCriteria : CriteriaBase
    {
        public int PartnerID { get; set; }

        public override string GetSettingKey() => "Odds_PartnerProfileSelByID";

        public override bool IsValid() => true;
    }
}