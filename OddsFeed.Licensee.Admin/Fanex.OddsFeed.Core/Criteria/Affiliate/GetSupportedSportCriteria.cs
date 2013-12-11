// --------------------------------------------------------------------------
// <copyright file="GetSupportedSportCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.3.6">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Framework.Data;

    /// <summary>
    /// Class GetSupportedSportCriteria.
    /// </summary>    
    public class GetSupportedSportCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Odds_SupportedSportSel";
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
