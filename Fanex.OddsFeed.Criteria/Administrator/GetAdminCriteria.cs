namespace Fanex.OddsFeed.Criteria.Administrator
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Data.Repository;
    using Fanex.Data;
    using Fanex.OddsFeed.Resources;

    public class GetAdminCriteria : CriteriaBase
    {
        [Required]
        public int SystemID { get; set; }

        [SpParam(SqlParamName = "RoleID")]
        public int RoleID { get; set; }

        [Required]
        [Display(Name = "Username", ResourceType = typeof(ViewResources))]
        [StringLength(50, MinimumLength = 2,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        [RegularExpression(@"^[\w\s]{1,}$",
            ErrorMessageResourceName = "InvalidNameValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        [SpParam(SqlParamName = "Username")]
        public string Username { get; set; }

        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public int RowCount { get; set; }

        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }
        public short? IsActive { get; set; }
        public short SortByID { get; set; }
        public bool IsSortAsc { get; set; }

        // Ignored
        [SpParam(Ignored = true)]
        public int UserId { get; set; }

        [SpParam(Ignored = true)]
        public int RoleName { get; set; }

        [SpParam(Ignored = true)]
        public string FirstName { get; set; }

        [SpParam(Ignored = true)]
        public string LastName { get; set; }

        [Display(Name = "Email", ResourceType = typeof(ViewResources))]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
            ErrorMessageResourceName = "EmailFormatValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        [SpParam(Ignored = true)]
        public string Email { get; set; }

        [SpParam(Ignored = true)]
        public string Creator { get; set; }

        [SpParam(Ignored = true)]
        public DateTime CreateDate { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_Admin_Sel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}