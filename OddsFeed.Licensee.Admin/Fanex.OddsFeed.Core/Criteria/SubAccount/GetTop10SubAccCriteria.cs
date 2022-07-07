namespace Fanex.OddsFeed.Core.Criteria.SubAccount
{
    using Data.Repository;

    public class GetTop10SubAccCriteria : CriteriaBase
    {
        public byte SystemId { get; set; }

        public int RoleId { get; set; }

        public string Username { get; set; }

        public override string GetSettingKey() => "Odds_Top10SubAccount_Sel";

        public override bool IsValid() => true;
    }
}