namespace Fanex.OddsFeed.Criteria.User
{
    using Fanex.Data.Repository;
    
    public class UserGetPermissionCriteria : CriteriaBase
    {
        public int UserID { get; set; }

        public short SystemID { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; } = string.Empty;

        public override string GetSettingKey() => "Odds_User_GetPermission";

        public override bool IsValid() => true;
    }
}