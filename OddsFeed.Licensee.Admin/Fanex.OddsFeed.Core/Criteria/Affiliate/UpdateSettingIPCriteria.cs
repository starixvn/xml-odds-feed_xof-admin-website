// --------------------------------------------------------------------------
// <copyright file="UpdateSettingIPCriteria.cs" company="Nexcel Solutions Vietnam">
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

    /// <summary>
    /// Class UpdateSettingIPCriteria.
    /// </summary>
    public class UpdateSettingIPCriteria : CriteriaBase
    {
        public UpdateSettingIPCriteria()
        {            
        }

        public int PartnerID { get; set; }

        [SpParam(SqlParamName = "IPFrom")]
        public string IpFrom { get; set; }

        [SpParam(SqlParamName = "IPTo")]
        public string IpTo { get; set; }

        [SpParam(SqlParamName = "IPFromOld")]
        public string IpFromOld { get; set; }

        [SpParam(SqlParamName = "IPToOld")]
        public string IpToOld { get; set; }      
      
        public override string GetSettingKey()
        {
            return "Odds_Partner_Upd_Setting_IP";
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