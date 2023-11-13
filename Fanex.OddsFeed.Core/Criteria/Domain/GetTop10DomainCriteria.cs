namespace Fanex.OddsFeed.Core.Criteria.Domain
{
    using Data.Repository;

    public class GetTop10DomainCriteria : CriteriaBase
    {
        public string DomainName { get; set; }
      
        public override string GetSettingKey() => "Odds_DomainList_Top10_Sel";

        public override bool IsValid() => true;
    }
}