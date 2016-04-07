// --------------------------------------------------------------------------
// <copyright file="UpdateActiveCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Class UpdateActiveCriteria.
    /// </summary>
    public class UpdateActiveCriteria : CriteriaBase
    {
        public UpdateActiveCriteria()
        {
            ErrCode = 0;
            ErrMsg = string.Empty;
        }

        public int PartnerID { get; set; }

        public bool IsAllowed { get; set; }

        public int Creator { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_Partner_Upd_Active";
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