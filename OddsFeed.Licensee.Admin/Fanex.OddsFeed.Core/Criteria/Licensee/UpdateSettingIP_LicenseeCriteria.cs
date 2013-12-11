// --------------------------------------------------------------------------
// <copyright file="UpdateSettingIP_LicenseeCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Licensee
{
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;    
    using Fanex.OddsFeed.Framework.Data;

    /// <summary>
    /// Class UpdateSettingIP_LicenseeCriteria.
    /// </summary>
    public class UpdateSettingIP_LicenseeCriteria : CriteriaBase
    {
        public UpdateSettingIP_LicenseeCriteria()
        {
        }

        [Required]
        public int Creator { get; set; }

        [Required]
        public byte SystemId { get; set; }

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
            return "Odds_Upd_Setting_IP_Licensee";
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
