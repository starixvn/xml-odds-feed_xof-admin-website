namespace Fanex.OddsFeed.Core.Criteria.Dashboard
{
    using System.ComponentModel.DataAnnotations;
    using Data.Repository;

    public class GetLoginInfoCriteria : CriteriaBase
    {
        [Required]
        public int UserID { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public int RowCount { get; set; }

        public override string GetSettingKey() => "Odds_Dashboard_LastLogin";

        public override bool IsValid() => true;
    }
}