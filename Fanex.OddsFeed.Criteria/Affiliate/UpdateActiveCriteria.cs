namespace Fanex.OddsFeed.Criteria.Affiliate
{
	public class UpdateActiveCriteria : BaseNonQueryCommand
	{
		public int PartnerID { get; set; }

		public bool IsAllowed { get; set; }

		public int Creator { get; set; }

		public override string GetSettingKey() => "Odds_Partner_Upd_Active";

		public override bool IsValid() => true;
	}
}