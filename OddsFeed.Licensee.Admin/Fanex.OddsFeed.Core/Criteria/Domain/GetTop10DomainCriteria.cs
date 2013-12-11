// --------------------------------------------------------------------------
// <copyright file="GetTop10DomainCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2013.03.05">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Domain
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
      
        public override string GetSettingKey()
        {
            return "Odds_DomainList_Top10_Sel";
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
