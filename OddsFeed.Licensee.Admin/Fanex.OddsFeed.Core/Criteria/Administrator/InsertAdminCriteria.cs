// --------------------------------------------------------------------------
// <copyright file="InsertAdminCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2013.03.05">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Administrator
{
    using System.ComponentModel.DataAnnotations;
    using Core.Web;
    using Framework.Data;

    /// <summary>
    /// Class InsertAdminCriteria.
    /// </summary>
    public class InsertAdminCriteria : CriteriaBase
    {        
        public InsertAdminCriteria()
        {
        }

        public int UserID { get; set; }

        [Required]
        [Display(Name = "Name", ResourceType = typeof(ViewResources))]
        [StringLength(50, MinimumLength = 2,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        [RegularExpression(@"^[\w\s]{1,}$",
            ErrorMessageResourceName = "InvalidNameValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        [SpParam(SqlParamName = "Username")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Password", ResourceType = typeof(ViewResources))]
        [StringLength(50, MinimumLength = 6,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        [SpParam(SqlParamName = "Password")]
        public string Password { get; set; }

        [Required]
        [SpParam(SqlParamName = "RoleID")]
        public int RoleID { get; set; }

        [StringLength(50,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        [SpParam(SqlParamName = "FirstName")]
        public string FirstName { get; set; }

        [StringLength(50,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        [SpParam(SqlParamName = "LastName")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email", ResourceType = typeof(ViewResources))]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
            ErrorMessageResourceName = "EmailFormatValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        [SpParam(SqlParamName = "Email")]
        public string Email { get; set; }

        [Required]
        [SpParam(SqlParamName = "ListSystemId")]
        public string System { get; set; }

        [Required]
        [SpParam(SqlParamName = "IsImport")]
        public bool IsImport { get; set; }

        [Required]
        public int Creator { get; set; }

        [Required]
        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
     
        public override string GetSettingKey()
        {
            return "Odds_Admin_Ins";
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
