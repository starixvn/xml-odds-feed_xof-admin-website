// --------------------------------------------------------------------------
// <copyright file="UpdatePasswordUserCriteria.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.3.14">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.User
{
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using Core.Web;
    using Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class UpdatePasswordUserCriteria.
    /// </summary>
    public class UpdatePasswordUserCriteria : CriteriaBase
    {
        public int UserID { get; set; }
       
        [Display(Name = "Old_Password", ResourceType = typeof(ViewResources))]
        public string OldPassword { get; set; }
        
        [Display(Name = "New_Password", ResourceType = typeof(ViewResources))]
        public string Password { get; set; }
        
        [Display(Name = "Confirm_Password", ResourceType = typeof(ViewResources))]      
        public string ConfirmPassword { get; set; }
        
        public int Creator { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }


        // Ignored
        [SpParam(Ignored = true)]
        public string PreController { get; set; }

        [SpParam(Ignored = true)]
        public string PreAction { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_UserPasswordUpd";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}