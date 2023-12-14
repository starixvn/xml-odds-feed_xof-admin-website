namespace Fanex.OddsFeed.Criteria.SubAccount
{
    using System.ComponentModel.DataAnnotations;
    using Fanex.Data;
    using Fanex.OddsFeed.Resources;

    public class UpdateSubAccCriteria : BaseNonQueryCommand
    {
        [SpParam(SqlParamName = "UserID")]
        public int SubAccountID { get; set; }

        [Display(Name = "First_Name", ResourceType = typeof(ViewResources))]
        public string FirstName { get; set; }

        [Display(Name = "Last_Name", ResourceType = typeof(ViewResources))]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email", ResourceType = typeof(ViewResources))]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
            ErrorMessageResourceName = "EmailFormatValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Is_Active", ResourceType = typeof(ViewResources))]
        public bool IsActive { get; set; }

        public string Ip { get; set; }

        [Required]
        [Display(Name = "Name", ResourceType = typeof(ViewResources))]
        [RegularExpression(@"^[\w\s]{1,}$",
            ErrorMessageResourceName = "InvalidNameValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        [SpParam(Ignored = true)]
        public string UserName { get; set; }

        public override string GetSettingKey() => "Odds_User_Upd";

        public override bool IsValid() => true;
    }
}