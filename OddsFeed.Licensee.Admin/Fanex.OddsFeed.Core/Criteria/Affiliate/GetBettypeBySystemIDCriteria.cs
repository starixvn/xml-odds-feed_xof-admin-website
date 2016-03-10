// --------------------------------------------------------------------------
// <copyright file="GetBettypeBySystemIDCriteria.cs" company="Nexcel Solutions Vietnam">
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
    using Data.Repository;
    /// <summary>
    /// Class GetBETTYPEBySystemIDCriteria.
    /// </summary>
    public class GetBettypeBySystemIDCriteria : CriteriaBase
    {
        public GetBettypeBySystemIDCriteria()
        {            
        }

        public byte SystemID { get; set; }
        
        public override string GetSettingKey()
        {
            return "Odds_BetTypeSel";
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
