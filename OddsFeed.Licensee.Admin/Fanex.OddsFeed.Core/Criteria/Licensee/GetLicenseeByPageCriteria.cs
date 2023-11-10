namespace Fanex.OddsFeed.Core.Criteria.Licensee
{
    using Data.Repository;

    public class GetLicenseeByPageCriteria : CriteriaBase
    {
        public string SystemName { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public int RowCount { get; set; }

        public short? IsActive { get; set; }
        public short SortByID { get; set; }
        public bool IsSortAsc { get; set; }

        public override string GetSettingKey() => "Odds_LicenseeSelByPage";

        public override bool IsValid() => true;
    }
}