namespace Fanex.OddsFeed.Criteria.Affiliate
{
    using Data.Repository;

    public class GetSportBySystemIDCriteria : CriteriaBase
    {
        public short SystemID { get; set; }

        public override string GetSettingKey() => "Odds_SportSel";

        public override bool IsValid() => true;
    }
}