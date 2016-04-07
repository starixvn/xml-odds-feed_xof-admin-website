// --------------------------------------------------------------------------
// <copyright file="GetTop10AffiliateCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.03.18">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class GetTop10AffiliateCriteria.
    /// </summary>
    public class GetTop10AffiliateCriteria : CriteriaBase
    {
        public GetTop10AffiliateCriteria()
        {
        }

        public byte SystemId { get; set; }

        public string Username { get; set; }
                
        public override string GetSettingKey()
        {
            return "Odds_Top10Partner_Sel";
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
