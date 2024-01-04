namespace Fanex.OddsFeed.Criteria.User
{
    using Fanex.Data.Repository;

    public class UserGetProfileCriteria : CriteriaBase
    {
        public int UserId { get; set; }
        public short SystemId { get; set; }
        public string IP { get; set; }
        public string NetSessionID { get; set; }
        public int OnUserId { get; set; }
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; } = string.Empty;

        public override string GetSettingKey() => "Odds_User_GetProfile";

        public override bool IsValid() => true;
    }
}