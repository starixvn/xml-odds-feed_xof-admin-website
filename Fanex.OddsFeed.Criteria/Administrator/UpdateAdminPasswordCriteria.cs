namespace Fanex.OddsFeed.Criteria.Administrator
{
	using System.ComponentModel.DataAnnotations;
	using Fanex.Data;
	using Fanex.OddsFeed.Resources;

	public class UpdateAdminPasswordCriteria : BaseNonQueryCommand
	{
		[Required]
		public int UserID { get; set; }

		[Display(Name = "Password", ResourceType = typeof(ViewResources))]
		[DataType(DataType.Password)]
		[SpParam(SqlParamName = "Password")]
		public string Password { get; set; }

		public int Creator { get; set; }

		public string IP { get; set; }

		// Ignored
		[SpParam(Ignored = true)]
		[Display(Name = "Confirm_Password", ResourceType = typeof(ViewResources))]
		[DataType(DataType.Password)]
		[Compare("Password")]
		public string ConfirmPassword { get; set; }

		public override string GetSettingKey()
		{
			return "Odds_Admin_UpdPwd";
		}

		/// <summary>
		/// Check data's constrains.
		/// </summary>
		/// <returns>True: valid; False: invalid.</returns>
		public override bool IsValid()
		{
			return true;
		}
	}
}
