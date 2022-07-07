namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using Data.Repository;

    public class GetTypeSettingByAffiliateIDCriteria : CriteriaBase
    {
        public string Type { get; set; }

        public int PartnerID { get; set; }

        public override string GetSettingKey() => "Odds_SystemSettingsSelByPartnerId";

        public override bool IsValid() => true;
    }
}