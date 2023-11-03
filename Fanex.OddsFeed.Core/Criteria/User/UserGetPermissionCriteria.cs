namespace Fanex.OddsFeed.Core.Criteria.User
{
    public class UserGetPermissionCriteria : ResultCriteria
    {
        public int UserID { get; set; }

        public byte SystemID { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey() => "Odds_User_GetPermission";

        public override bool IsValid() => true;
    }
}