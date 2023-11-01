namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using Data.Repository;

    public class GetSubAccBySystemIDCriteria : CriteriaBase
    {
        public short SystemId { get; set; }

        public byte RoleId { get; set; }

        public override string GetSettingKey() => "Odds_User_GetBySystemIdAndRoleId";

        public override bool IsValid() => true;
    }
}