// --------------------------------------------------------------------------
// <copyright file="GetSystemCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.SubAccount
{
    using System.Diagnostics.CodeAnalysis;
    using Framework.Data;

    /// <summary>
    /// Class GetSystemCriteria.
    /// </summary>
    public class GetSystemCriteria : CriteriaBase
    {
        public GetSystemCriteria()
        {
        }
        
        public override string GetSettingKey()
        {
            return "Odds_System_Sel";
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
