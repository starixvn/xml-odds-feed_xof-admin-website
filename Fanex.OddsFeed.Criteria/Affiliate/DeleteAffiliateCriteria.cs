namespace Fanex.OddsFeed.Criteria.Affiliate
{
	public class DeleteAffiliateCriteria : BaseNonQueryCommand
	{
		public int PartnerID { get; set; }

		public int Creator { get; set; }

		public override string GetSettingKey() => "Odds_Partner_Del";

		public override bool IsValid() => true;
	}
}