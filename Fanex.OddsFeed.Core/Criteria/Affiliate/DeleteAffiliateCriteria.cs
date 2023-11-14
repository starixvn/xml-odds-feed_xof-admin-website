namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using Data.Repository;

    public class DeleteAffiliateCriteria : BaseCommand
    {
        public int PartnerID { get; set; }

        public int Creator { get; set; }

        public override string GetSettingKey() => "Odds_Partner_Del";

        public override bool IsValid() => true;
    }
}