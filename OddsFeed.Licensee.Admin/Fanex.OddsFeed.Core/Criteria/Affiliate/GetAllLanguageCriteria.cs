namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using Data.Repository;

    public class GetAllLanguageCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Odds_Language_GetAll";

        public override bool IsValid() => true;
    }
}