namespace Fanex.OddsFeed.Core.Criteria.Administrator
{
    using Data.Repository;

    public class GetSystemByUserIDCriteria : CriteriaBase
    {
        public int UserID { get; set; }

        public bool IgnoreIsActive { get; set; }
        
        public override string GetSettingKey()
        {
            return "Odds_System_SelByUserID";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}