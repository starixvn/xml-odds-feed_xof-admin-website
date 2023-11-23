namespace Fanex.OddsFeed.Criteria.Administrator
{
    using Data.Repository;

    public class GetOptionalSystemBySubAccountIDCriteria : CriteriaBase
    {
        public int UserID { get; set; }

        public int SubAccountID { get; set; }
        
        public override string GetSettingKey() => "Odds_System_SelByUserIDAndSubAccountID";

        public override bool IsValid() => true;
    }
}
