namespace Fanex.OddsFeed.Core.Criteria.SubAccount
{
    using Data.Repository;

    public class GetSubAccByNameCriteria : CriteriaBase
    {
        public short SystemId { get; set; }

        public byte RoleId { get; set; }

        public string Username { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public int RowCount { get; set; }

        public short? IsActive { get; set; }
        public short SortByID { get; set; }
        public bool IsSortAsc { get; set; }

        public override string GetSettingKey() => "Odds_User_SelByName_SystemId_RoleId";

        public override bool IsValid() => true;
    }
}