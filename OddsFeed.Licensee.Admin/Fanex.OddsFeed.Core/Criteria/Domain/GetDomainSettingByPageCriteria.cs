namespace Fanex.OddsFeed.Core.Criteria.Domain
{
    using Data.Repository;

    public class GetDomainSettingByPageCriteria : CriteriaBase
    {
        public short DomainID { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public int RowCount { get; set; }

        public override string GetSettingKey() => "Odds_DomainSettingSelByPage";

        public override bool IsValid() => true;
    }
}