namespace Fanex.OddsFeed.Criteria.Dashboard
{
    using System.ComponentModel.DataAnnotations;
    using Data.Repository;

    public class GetAccountStatisticsCriteria : CriteriaBase
    {
        [Required]
        public int SystemID { get; set; }

        public override string GetSettingKey() => "Odds_Dashboard_AccountStatistics";

        public override bool IsValid() => true;
    }
}