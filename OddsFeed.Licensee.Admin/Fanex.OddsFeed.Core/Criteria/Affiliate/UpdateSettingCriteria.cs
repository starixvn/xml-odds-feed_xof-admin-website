// --------------------------------------------------------------------------
// <copyright file="UpdateSettingCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.01.09">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class UpdateSettingCriteria.
    /// </summary>    
    public class UpdateSettingCriteria : CriteriaBase
    {
        public UpdateSettingCriteria()
        {
            ErrCode = 0;
            ErrMsg = string.Empty;
        }

        public int PartnerID { get; set; }

        [SpParam(SqlParamName = "ListDomainId")]
        public string Domains { get; set; }

        [SpParam(SqlParamName = "ListIPFrom")]
        public string IpsFrom { get; set; }

        [SpParam(SqlParamName = "ListIPTo")]
        public string IpsTo { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_Partner_Upd_Setting";
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