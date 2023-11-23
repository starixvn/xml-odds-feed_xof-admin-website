namespace Fanex.OddsFeed.Criteria.User
{
    using Data.Repository;

    public class UserReCheckInUserCriteria : CriteriaBase
    {
        public int OnUserId { get; set; }

        public bool IsKickedOut { get; set; }

        public override string GetSettingKey() => "Odds_User_ReCheckIn";

        public override bool IsValid() => true;
    }
}