namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using Data.Repository;

    public class GetSupportedBettypeCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Odds_SupportedBetTypeSel";

        public override bool IsValid() => true;
    }
}