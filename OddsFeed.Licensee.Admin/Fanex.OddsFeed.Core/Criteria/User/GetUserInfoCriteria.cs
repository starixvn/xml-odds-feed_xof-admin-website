namespace Fanex.OddsFeed.Core.Criteria.User
{
    using Data.Repository;

    public class GetUserInfoCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public override string GetSettingKey() => "Odds_User_GetInfo";

        public override bool IsValid() => true;
    }
}