namespace Fanex.OddsFeed.Criteria.Affiliate
{
    using Data.Repository;

    public class GetDomainBySystemIDCriteria : CriteriaBase
    {
        public short SystemID { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsOddsDomain { get; set; }

        public override string GetSettingKey() => "Odds_Domain_Sel";

        public override bool IsValid() => true;
    }
}