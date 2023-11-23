namespace Fanex.OddsFeed.Criteria.Affiliate
{
    using Data.Repository;

    public class GetTop10DomainCriteria : CriteriaBase
    {
        public short SystemId { get; set; }

        public string DomainName { get; set; }

        public override string GetSettingKey() => "Odds_Top10Domain_Sel";

        public override bool IsValid() => true;
    }
}