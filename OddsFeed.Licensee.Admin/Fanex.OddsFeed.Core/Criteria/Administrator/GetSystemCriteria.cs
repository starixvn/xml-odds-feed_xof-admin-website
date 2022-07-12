namespace Fanex.OddsFeed.Core.Criteria.Administrator
{
    using Data.Repository;

    public class GetSystemCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Odds_System_Sel";

        public override bool IsValid() => true;
    }
}