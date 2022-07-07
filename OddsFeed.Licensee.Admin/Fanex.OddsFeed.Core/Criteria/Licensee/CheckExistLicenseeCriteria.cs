namespace Fanex.OddsFeed.Core.Criteria.Licensee
{
    using Data.Repository;

    public class CheckExistLicenseeCriteria : CriteriaBase
    {
        public string SystemName { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public int RowCount { get; set; }

        public override string GetSettingKey() => "Odds_Licensee_CheckExistLicensee";

        public override bool IsValid() => true;
    }
}