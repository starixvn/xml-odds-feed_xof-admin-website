namespace Fanex.OddsFeed.Core.Criteria.User
{
	public class UserGetProfileCriteria : ResultCriteria
	{
		public int UserId { get; set; }

		public short SystemId { get; set; }

		public string IP { get; set; }

		public string NetSessionID { get; set; }

		public int OnUserId { get; set; }

		public override string GetSettingKey() => "Odds_User_GetProfile";

		public override bool IsValid() => true;
	}
}