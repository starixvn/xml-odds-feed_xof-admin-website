// --------------------------------------------------------------------------
// <copyright file="GetAdminCriteria.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
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
    using Data.Repository;
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


        // Ignored
        [SpParam(Ignored = true)]
        public int UserID { get; set; }

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
        public bool IsActive { get; set; }

        [SpParam(Ignored = true)]
        public DateTime CreateDate { get; set; }             

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
