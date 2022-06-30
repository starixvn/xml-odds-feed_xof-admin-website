namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using Data.Repository;

    public class GetTypeSettingCriteria : CriteriaBase
    {
        public byte SystemID { get; set; }

        public string Type { get; set; }

        public override string GetSettingKey() => "Odds_SystemSettingsSel";

        public override bool IsValid() => true;
    }
}