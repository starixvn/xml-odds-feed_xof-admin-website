namespace Fanex.OddsFeed.Core.Criteria.Domain
{
    using Data.Repository;

    public class GetDomainByPageCriteria : CriteriaBase
    {
        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public int RowCount { get; set; }

        public string DomainName { get; set; }

        [SpParam(Ignored = true)]
        public int DomainId { get; set; }

        public override string GetSettingKey() => "Odds_DomainSelByPage";

        public override bool IsValid() => true;
    }
}