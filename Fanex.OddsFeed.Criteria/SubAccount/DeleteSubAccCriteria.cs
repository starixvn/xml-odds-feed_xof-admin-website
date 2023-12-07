namespace Fanex.OddsFeed.Criteria.SubAccount
{
    using Fanex.Data;

    public class DeleteSubAccCriteria : BaseNonQueryCommand
    {
        [SpParam(SqlParamName = "UserID")]
        public int SubAccountID { get; set; }

        public override string GetSettingKey() => "Odds_User_Del";

        public override bool IsValid() => true;
    }
}