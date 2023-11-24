namespace Fanex.OddsFeed.Criteria.SubAccount
{
    using Fanex.Data;
    public class UpdateActiveSubAccCriteria : BaseCommand
    {
        [SpParam(SqlParamName = "UserID")]
        public int SubAccountID { get; set; }

        public bool IsActive { get; set; }

        public override string GetSettingKey() => "Odds_User_Upd_Active";

        public override bool IsValid() => true;
    }
}