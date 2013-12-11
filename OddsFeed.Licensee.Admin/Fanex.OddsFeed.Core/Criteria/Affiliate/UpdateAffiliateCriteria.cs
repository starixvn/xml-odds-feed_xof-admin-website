// --------------------------------------------------------------------------
// <copyright file="UpdateAffiliateCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Class UpdateAffiliateCriteria.
    /// </summary>
    public class UpdateAffiliateCriteria : CriteriaBase
    {
        public UpdateAffiliateCriteria()
        {
        }

        public int PartnerID { get; set; }

        public int UpdatedBy { get; set; }

        [Required]
        [Display(Name = "Name", ResourceType = typeof(ViewResources))]
        [StringLength(50, MinimumLength = 2,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        [RegularExpression(@"^[\w\s]{1,}$",
            ErrorMessageResourceName = "InvalidNameValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        [SpParam(Ignored = true)]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Email", ResourceType = typeof(ViewResources))]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
            ErrorMessageResourceName = "EmailFormatValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        [SpParam(SqlParamName = "ContactEmail")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Website", ResourceType = typeof(ViewResources))]
        [SpParam(SqlParamName = "Site")]
        public string Website { get; set; }

        [Display(Name = "Note", ResourceType = typeof(ViewResources))]
        [SpParam(SqlParamName = "Remark")]
        public string Note { get; set; }

        ////[Display(Name = "Request_mail_content", ResourceType = typeof(ViewResources))]        
        ////public string Content { get; set; }

        /// <summary>
        /// Get the reference SP's Name which is specified in Config file.
        /// </summary>
        /// <returns>The key of store procedure.</returns>
        public override string GetSettingKey()
        {
            return "Odds_Partner_Upd_Info";
        }

        /// <summary>
        /// Check data's constrains.
        /// </summary>
        /// <returns>True: valid; False: invalid..</returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}