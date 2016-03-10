// --------------------------------------------------------------------------
// <copyright file="ResetPassSubAccCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.01.09">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Fanex.OddsFeed.Core.Web;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class ResetPassSubACCCriteria.
    /// </summary>
    public class ResetPassSubAccCriteria : CriteriaBase
    {
        public ResetPassSubAccCriteria()
        {
            ErrCode = 0;
            ErrMsg = string.Empty;
        }

        [SpParam(SqlParamName = "UserID")]
        public int SubAccountID { get; set; }

        [Display(Name = "New_Password", ResourceType = typeof(ViewResources))]
        public string Password { get; set; }

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
            return "Odds_User_ResetPass";
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