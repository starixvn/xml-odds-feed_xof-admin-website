namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using Data.Repository;

    public class GetLanguageBySystemIDCriteria : CriteriaBase
    {
        public byte SystemID { get; set; }

        public override string GetSettingKey() => "Odds_LanguageSel";

        public override bool IsValid() => true;
    }
}