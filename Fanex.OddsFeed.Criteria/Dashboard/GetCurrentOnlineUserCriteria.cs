namespace Fanex.OddsFeed.Criteria.Dashboard
{
    using System.ComponentModel.DataAnnotations;
    using Data.Repository;

    public class GetCurrentOnlineUserCriteria : CriteriaBase
    {
        [Required]
        public int UserID { get; set; }

        [Required]
        public int RoleID { get; set; }

        [Required]
        public int SystemID { get; set; }

        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public int RowCount { get; set; }

        public override string GetSettingKey() => "Odds_Dashboard_CurrentOnlUsers";

        public override bool IsValid() => true;
    }
}