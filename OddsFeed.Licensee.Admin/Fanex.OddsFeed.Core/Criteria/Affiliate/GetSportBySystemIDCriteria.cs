// --------------------------------------------------------------------------
// <copyright file="GetSportBySystemIDCriteria.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
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
    /// Class GetSportBySystemIDCriteria.
    /// </summary>
    public class GetSportBySystemIDCriteria : CriteriaBase
    {
        public GetSportBySystemIDCriteria()
        {            
        }

        public byte SystemID { get; set; }
        
        public override string GetSettingKey()
        {
            return "Odds_SportSel";
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
