namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using Data.Repository;

    public class CheckExistAffiliateCriteria : CriteriaBase
    {
        public byte SystemId { get; set; }

        public string UserName { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public int RowCount { get; set; }

        public override string GetSettingKey() => "Odds_Partner_CheckExist";

        public override bool IsValid() => true;
    }
}