
namespace Fanex.OddsFeed.Criteria.Affiliate
{
    using Fanex.Data.Repository;

    public class GetTypeSettingCriteria : CriteriaBase
    {
        public short SystemID { get; set; }

        public string Type { get; set; }

        public override string GetSettingKey() => "Odds_SystemSettingsSel";

        public override bool IsValid() => true;
    }
}