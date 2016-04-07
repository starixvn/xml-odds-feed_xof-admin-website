// --------------------------------------------------------------------------
// <copyright file="DeleteSettingIP_LicenseeCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Licensee
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class DeleteSettingIP_LicenseeCriteria.
    /// </summary>
    public class DeleteSettingIP_LicenseeCriteria : CriteriaBase
    {
        public DeleteSettingIP_LicenseeCriteria()
        {
        }

        public int SystemId { get; set; }

        public int Creator { get; set; }

        public string IP { get; set; }

        public string IPFromOld { get; set; }

        public string IPToOld { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        /// <summary>
        /// Get the reference SP's Name which is specified in Config file.
        /// </summary>
        /// <returns>String "Odds_Delete_Setting_IP_Licensee".</returns>
        public override string GetSettingKey()
        {
            return "Odds_Delete_Setting_IP_Licensee";
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
