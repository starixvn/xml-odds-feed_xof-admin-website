namespace Fanex.OddsFeed.Criteria.SubAccount
{
    using Data.Repository;

    public class CheckExistSubAccountCriteria : CriteriaBase
    {
        public byte RoleId { get; set; }

        public string Username { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public int RowCount { get; set; }

        public override string GetSettingKey() => "Odds_SubAccount_CheckExist";

        public override bool IsValid() => true;
    }
}