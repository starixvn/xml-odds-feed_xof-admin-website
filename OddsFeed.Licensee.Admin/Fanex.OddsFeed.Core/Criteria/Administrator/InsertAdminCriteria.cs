namespace Fanex.OddsFeed.Core.Criteria.Administrator
{
    using System.ComponentModel.DataAnnotations;
    using Core.Web;
    using Data.Repository;

    public class InsertAdminCriteria : CriteriaBase
    {
        public int UserID { get; set; }

        [Required]
        [Display(Name = "Name", ResourceType = typeof(ViewResources))]
        [StringLength(50, MinimumLength = 2,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        [RegularExpression(@"^[\w\s]{1,}$",
            ErrorMessageResourceName = "InvalidNameValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Password", ResourceType = typeof(ViewResources))]
        [StringLength(50, MinimumLength = 6,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        public string Password { get; set; }

        [StringLength(50,
           ErrorMessageResourceName = "InputLengthValidationMsg",
           ErrorMessageResourceType = typeof(ViewResources))]
        public string FirstName { get; set; }

        [StringLength(50,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        public string LastName { get; set; }

        [Required]
        public int RoleID { get; set; }

        [Required]
        [Display(Name = "Email", ResourceType = typeof(ViewResources))]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
            ErrorMessageResourceName = "EmailFormatValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        public string Email { get; set; }

        [Required]
        public bool IsImport { get; set; }

        [Required]
        [SpParam(SqlParamName = "ListSystemId")]
        public string System { get; set; }

        [Required]
        public int Creator { get; set; }

        [Required]
        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Odds_Admin_Ins";

        public override bool IsValid() => true;
    }
}