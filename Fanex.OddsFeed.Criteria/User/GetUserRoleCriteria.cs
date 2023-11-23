namespace Fanex.OddsFeed.Criteria.User
{
    using Data.Repository;

    public class GetUserRoleCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Odds_Role_Sel";

        public override bool IsValid() => true;
    }
}