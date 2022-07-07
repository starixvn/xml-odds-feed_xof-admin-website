namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using Data.Repository;

    public class GetTop10AffiliateCriteria : CriteriaBase
    {
        public byte SystemId { get; set; }

        public string Username { get; set; }

        public override string GetSettingKey() => "Odds_Top10Partner_Sel";

        public override bool IsValid() => true;
    }
}