// --------------------------------------------------------------------------
// <copyright file="GetTop10DomainCriteria.cs" company="Nexcel Solutions Vietnam">
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

    /// <summary>
    /// Class GetTop10DomainCriteria.
    /// </summary>
    public class GetTop10DomainCriteria : CriteriaBase
    {
        public GetTop10DomainCriteria()
        {
        }

        public string DomainName { get; set; }

        public byte SystemId { get; set; }
                
        public override string GetSettingKey()
        {
            return "Odds_Top10Domain_Sel";
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
