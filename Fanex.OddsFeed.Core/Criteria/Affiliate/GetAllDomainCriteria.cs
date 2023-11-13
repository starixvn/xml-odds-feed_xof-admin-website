namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using Data.Repository;
    using Fanex.Data;

    public class GetAllDomainCriteria : CriteriaBase
    {
        public bool? IsActive { get; set; }

        public bool? IsOddsDomain { get; set; }

        [SpParam(Ignored = true)]
        public byte SystemID { get; set; }

        public override string GetSettingKey() => "Odds_Domain_GetAll";

        public override bool IsValid() => true;
    }
}