namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using Data.Repository;

    public class GetAllDomainCriteria : CriteriaBase
    {
        public bool? IsActive { get; set; }

        public bool? IsOddsDomain { get; set; }

        // Ignored
        [SpParam(Ignored = true)]
        public byte SystemID { get; set; }

        public override string GetSettingKey() => "Odds_Domain_GetAll";

        public override bool IsValid() => true;
    }
}