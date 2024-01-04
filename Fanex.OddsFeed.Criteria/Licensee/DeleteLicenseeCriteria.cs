namespace Fanex.OddsFeed.Criteria.Licensee
{
	public class DeleteLicenseeCriteria : BaseNonQueryCommand
	{
		public short SystemID { get; set; }

		public int Creator { get; set; }

		public string IP { get; set; }

		public override string GetSettingKey() => "Odds_LicenseeDel";

		public override bool IsValid() => true;
	}
}