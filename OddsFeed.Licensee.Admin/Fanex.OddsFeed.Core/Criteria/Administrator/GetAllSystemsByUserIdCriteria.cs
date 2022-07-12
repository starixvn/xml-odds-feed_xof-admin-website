namespace Fanex.OddsFeed.Core.Criteria.Administrator
{
    using Data.Repository;

    public class GetAllSystemsByUserIdCriteria : CriteriaBase
    {
        public byte UserRole { get; set; }

        public int UserId { get; set; }

        public override string GetSettingKey() => "Odds_AdminSystem_SelAll";

        public override bool IsValid() => true;
    }
}