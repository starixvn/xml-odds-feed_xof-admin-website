namespace Fanex.OddsFeed.Core.Criteria.Administrator
{
    using Data.Repository;

    public class GetTop10AdminCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public override string GetSettingKey() => "Odds_Top10Admin_Sel";

        public override bool IsValid() => true;
    }
}
