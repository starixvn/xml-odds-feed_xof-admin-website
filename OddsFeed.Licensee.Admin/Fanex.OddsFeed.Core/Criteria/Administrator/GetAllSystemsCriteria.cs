namespace Fanex.OddsFeed.Core.Criteria.Administrator
{
    using Data.Repository;

    public class GetAllSystemsCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Odds_System_SelAll";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}