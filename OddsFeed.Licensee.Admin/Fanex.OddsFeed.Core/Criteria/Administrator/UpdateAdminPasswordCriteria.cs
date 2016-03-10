// --------------------------------------------------------------------------
// <copyright file="UpdateAdminPasswordCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2013.03.05">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Administrator
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Core.Web;
    using Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class UpdateAdminPasswordCriteria.
    /// </summary>
    public class UpdateAdminPasswordCriteria : CriteriaBase
    {  
        public UpdateAdminPasswordCriteria()
        {
        }

        [Required]
        public int UserID { get; set; }

        [Display(Name = "Password", ResourceType = typeof(ViewResources))]
        [SpParam(SqlParamName = "Password")]
        public string Password { get; set; }        

        public int Creator { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }


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
