// --------------------------------------------------------------------------
// <copyright file="GetAffiliateBySystemIDCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Framework.Data;

    /// <summary>
    /// Class GetAffiliateBySystemIDCriteria.
    /// </summary>
    public class GetAffiliateBySystemIDCriteria : CriteriaBase
    {
        public GetAffiliateBySystemIDCriteria()
        {            
        }

        public byte SystemId { get; set; }

        /// <summary>
        /// Get the reference SP's Name which is specified in Config file.
        /// </summary>
        /// <returns>The key of store procedure "Odds_Partner_GetBySystemId".</returns>
        public override string GetSettingKey()
        {
            return "Odds_Partner_GetBySytemId";
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
