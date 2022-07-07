namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using Data.Repository;

    public class DeleteSubAccCriteria : ResultCriteria
    {
        [SpParam(SqlParamName = "UserID")]
        public int SubAccountID { get; set; }

        public override string GetSettingKey() => "Odds_User_Del";

        public override bool IsValid() => true;
    }
}