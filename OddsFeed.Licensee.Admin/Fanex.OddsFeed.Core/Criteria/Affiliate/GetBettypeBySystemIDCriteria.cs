namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using Data.Repository;

    public class GetBettypeBySystemIDCriteria : CriteriaBase
    {
        public short SystemID { get; set; }

        public override string GetSettingKey() => "Odds_BetTypeSel";

        public override bool IsValid() => true;
    }
}