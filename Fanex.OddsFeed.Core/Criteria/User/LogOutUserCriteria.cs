namespace Fanex.OddsFeed.Core.Criteria.User
{
    using Data.Repository;

    public class LogOutUserCriteria : CriteriaBase
    {
        public int OnUserId { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_User_Logout";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}