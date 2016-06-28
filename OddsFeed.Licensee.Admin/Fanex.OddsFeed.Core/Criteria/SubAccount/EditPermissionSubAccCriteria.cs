// --------------------------------------------------------------------------
// <copyright file="EditPermissionSubAccCriteria.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
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
    using Data.Repository;
    /// <summary>
    /// Class EditPermissionSubACCCriteria.
    /// </summary>
    public class EditPermissionSubAccCriteria : CriteriaBase
    {
        public EditPermissionSubAccCriteria()
        {
            ErrCode = 0;
            ErrMsg = string.Empty;
        }

        [SpParam(SqlParamName = "UserID")]
        public int SubAccountID { get; set; }

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

        public int UpdatedBy { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        // Ignored
        [SpParam(Ignored = true)]
        public string SubAccountName { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_User_EditPermission";
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