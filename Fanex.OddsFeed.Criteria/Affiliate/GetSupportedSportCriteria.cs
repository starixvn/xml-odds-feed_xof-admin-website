namespace Fanex.OddsFeed.Criteria.Affiliate
{
    using Data.Repository;

    public class GetSupportedSportCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Odds_SupportedSportSel";

        public override bool IsValid() => true;
    }
}