namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using Data.Repository;

    public class UpdateActiveSubAccCriteria : ResultCriteria
    {
        [SpParam(SqlParamName = "UserID")]
        public int SubAccountID { get; set; }

        public bool IsActive { get; set; }

        public override string GetSettingKey() => "Odds_User_Upd_Active";

        public override bool IsValid() => true;
    }
}