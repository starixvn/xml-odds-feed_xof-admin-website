namespace Fanex.OddsFeed.Criteria.SubAccount
{
    using Fanex.Data;

    public class DeleteSubAccCriteria : BaseCommand
    {
        [SpParam(SqlParamName = "UserID")]
        public int SubAccountID { get; set; }

        public override string GetSettingKey() => "Odds_User_Del";

        public override bool IsValid() => true;
    }
}