// --------------------------------------------------------------------------
// <copyright file="InsertSubAccCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.01.09">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Core.Web;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class InsertSubACCCriteria.
    /// </summary>
    public class InsertSubAccCriteria : Framework.Data.CriteriaBase
    {
        public InsertSubAccCriteria()
        {
        }

        public int SubAccountID { get; set; }

        public int SystemID { get; set; }

        public byte RoleID { get; set; }

        public int Creator { get; set; }

        public string Ip { get; set; }

        #region step1

        [Required]
        [Display(Name = "Name", ResourceType = typeof(ViewResources))]
        [StringLength(50, MinimumLength = 2,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        [RegularExpression(@"^[\w\s]{1,}$",
            ErrorMessageResourceName = "InvalidNameValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Password", ResourceType = typeof(ViewResources))]
        [StringLength(50, MinimumLength = 6,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        public string Password { get; set; }

        [Display(Name = "First_Name", ResourceType = typeof(ViewResources))]
        [StringLength(50, MinimumLength = 2,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        public string FirstName { get; set; }

        [Display(Name = "Last_Name", ResourceType = typeof(ViewResources))]
        [StringLength(50, MinimumLength = 2,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
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

        #endregion

        #region Step2

        [Required]
        [SpParam(SqlParamName = "SystemIDList")]
        public string Systems { get; set; }

        public string[] SystemArray
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Systems))
                {
                    return null;
                }

                return Systems.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }

            set
            {
                Systems = Utilities.StringArrayToString(value);
            }
        }

        #endregion

        #region step3

        [Display(Name = "Odds_type", ResourceType = typeof(ViewResources))]
        public bool OddsTypeFlag
        {
            get;
            set;
        }

        [Display(Name = "Odds_format", ResourceType = typeof(ViewResources))]
        public bool OddsStyleFlag
        {
            get;
            set;
        }

        [Display(Name = "Languages", ResourceType = typeof(ViewResources))]
        public bool LanguageListFlag
        {
            get;
            set;
        }

        [Display(Name = "Sports", ResourceType = typeof(ViewResources))]
        public bool SportIDListFlag
        {
            get;
            set;
        }

        [Display(Name = "Countries", ResourceType = typeof(ViewResources))]
        public bool CountryCodeListFlag
        {
            get;
            set;
        }

        [Display(Name = "Markets", ResourceType = typeof(ViewResources))]
        public bool BetTypeIDListFlag
        {
            get;
            set;
        }

        #endregion
        
        public override string GetSettingKey()
        {
            return "Odds_User_Ins";
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