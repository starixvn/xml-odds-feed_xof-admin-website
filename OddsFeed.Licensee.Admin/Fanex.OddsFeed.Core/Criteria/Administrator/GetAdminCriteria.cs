// --------------------------------------------------------------------------
// <copyright file="GetAdminCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2013.03.05">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Administrator
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Core.Web;
    using Framework.Data;

    /// <summary>
    /// Class GetAdminCriteria.
    /// </summary>
    public class GetAdminCriteria : CriteriaBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAdminCriteria"/> class.
        /// </summary>
        public GetAdminCriteria()
        {
        }

        public int UserID { get; set; }

        [Required]
        public int SystemID { get; set; }

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
        
        [SpParam(SqlParamName = "RoleID")]
        public int RoleID { get; set; }

        [SpParam(SqlParamName = "RoleName")]
        public int RoleName { get; set; }

        [SpParam(SqlParamName = "FirstName")]
        public string FirstName { get; set; }

        [SpParam(SqlParamName = "LastName")]
        public string LastName { get; set; }

        [Display(Name = "Email", ResourceType = typeof(ViewResources))]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
            ErrorMessageResourceName = "EmailFormatValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        [SpParam(SqlParamName = "Email")]
        public string Email { get; set; }
        
        public string Creator { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; } 

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
        
        public override string GetSettingKey()
        {
            return "Odds_Admin_Sel";
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
