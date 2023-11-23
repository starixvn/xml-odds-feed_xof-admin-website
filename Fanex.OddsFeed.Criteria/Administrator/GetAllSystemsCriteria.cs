namespace Fanex.OddsFeed.Criteria.Administrator
{
    using Data.Repository;

    public class GetAllSystemsCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Odds_System_SelAll";

        public override bool IsValid() => true;
    }
}