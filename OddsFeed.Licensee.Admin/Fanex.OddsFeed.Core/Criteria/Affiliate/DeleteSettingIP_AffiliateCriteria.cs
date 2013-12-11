// --------------------------------------------------------------------------
// <copyright file="DeleteSettingIP_AffiliateCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2012.12.07">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Licensee
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Framework.Data;

    /// <summary>
    /// Class DeleteSettingIP_AffiliateCriteria.
    /// </summary>
    public class DeleteSettingIP_AffiliateCriteria : CriteriaBase
    {
        public DeleteSettingIP_AffiliateCriteria()
        {
        }

        public int PartnerId { get; set; }

        public string IPFromOld { get; set; }

        public string IPToOld { get; set; }

        public string IP { get; set; }

        public int Creator { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_Delete_Setting_IP_Partner";
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
