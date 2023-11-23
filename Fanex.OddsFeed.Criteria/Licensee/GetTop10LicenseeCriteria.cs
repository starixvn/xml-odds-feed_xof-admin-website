namespace Fanex.OddsFeed.Criteria.Affiliate
{
    using Data.Repository;

    public class GetTop10LicenseeCriteria : CriteriaBase
    {
        public string Systemname { get; set; }
 
        public override string GetSettingKey() => "Odds_Top10System_Sel";

        public override bool IsValid() => true;
    }
}