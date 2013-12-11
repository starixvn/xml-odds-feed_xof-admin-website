// --------------------------------------------------------------------------
// <copyright file="UpdateSubAccCriteria.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.OddsFeed.Core.Web;
    using Fanex.OddsFeed.Framework.Data;

    /// <summary>
    /// Class UpdateSubACCCriteria.
    /// </summary>
    public class UpdateSubAccCriteria : CriteriaBase
    {
        public UpdateSubAccCriteria()
        {            
        }

        [SpParam(SqlParamName = "UserID")]
        public int SubAccountID { get; set; }

        [Required]
        [Display(Name = "Name", ResourceType = typeof(ViewResources))]
        ////[StringLength(50, MinimumLength = 2,
        ////    ErrorMessageResourceName = "InputLengthValidationMsg",
        ////    ErrorMessageResourceType = typeof(ViewResources))]
        [RegularExpression(@"^[\w\s]{1,}$",
            ErrorMessageResourceName = "InvalidNameValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        [SpParam(Ignored = true)]
        public string UserName { get; set; }        
        
        [Display(Name = "First_Name", ResourceType = typeof(ViewResources))]
        ////[StringLength(50, MinimumLength = 2,
        ////    ErrorMessageResourceName = "InputLengthValidationMsg",
        ////    ErrorMessageResourceType = typeof(ViewResources))]                
        public string FirstName { get; set; }

        [Display(Name = "Last_Name", ResourceType = typeof(ViewResources))]
        ////[StringLength(50, MinimumLength = 2,
        ////    ErrorMessageResourceName = "InputLengthValidationMsg",
        ////    ErrorMessageResourceType = typeof(ViewResources))]        
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
              
        public override string GetSettingKey()
        {
            return "Odds_User_Upd";
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