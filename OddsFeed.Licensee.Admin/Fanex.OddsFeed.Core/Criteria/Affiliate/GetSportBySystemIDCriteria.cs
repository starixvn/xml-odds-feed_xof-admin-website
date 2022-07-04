namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using Data.Repository;

    public class GetSportBySystemIDCriteria : CriteriaBase
    {
        public byte SystemID { get; set; }

        public override string GetSettingKey() => "Odds_SportSel";

        public override bool IsValid() => true;
    }
}