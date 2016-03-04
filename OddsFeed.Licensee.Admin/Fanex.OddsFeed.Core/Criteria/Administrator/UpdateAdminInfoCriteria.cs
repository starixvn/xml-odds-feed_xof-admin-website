// --------------------------------------------------------------------------
// <copyright file="UpdateAdminInfoCriteria.cs" company="Nexcel Solutions Vietnam">
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
    using Data.Repository;
    /// <summary>
    /// Class UpdateAdminInfoCriteria.
    /// </summary>
    public class UpdateAdminInfoCriteria : Framework.Data.CriteriaBase
    {
        public UpdateAdminInfoCriteria()
        {
        }

        [Required]
        [SpParam(SqlParamName = "AdminID")]
        public int UserID { get; set; }
        
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

        [Required]
        [SpParam(SqlParamName = "RoleID")]
        public int RoleID { get; set; }

        [StringLength(50, MinimumLength = 2,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        [SpParam(SqlParamName = "FirstName")]
        public string FirstName { get; set; }

        [StringLength(50, MinimumLength = 2,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        [SpParam(SqlParamName = "LastName")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email", ResourceType = typeof(ViewResources))]
        [StringLength(50, MinimumLength = 4,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
            ErrorMessageResourceName = "EmailFormatValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        [SpParam(SqlParamName = "Email")]
        public string Email { get; set; }

        [Required]
        public int Creator { get; set; }

        [Required]
        public string IP { get; set; }
        
        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
   
        public override string GetSettingKey()
        {
            return "Odds_Admin_UpdInfo";
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
