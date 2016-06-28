// --------------------------------------------------------------------------
// <copyright file="GetAllSystemsCriteria.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2014.05.20">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Administrator
{
    using System.Diagnostics.CodeAnalysis;
    using Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class GetAllSystemsCriteria.
    /// </summary>
    public class GetAllSystemsCriteria : CriteriaBase
    {
        public GetAllSystemsCriteria()
        {            
        }
        
        public override string GetSettingKey()
        {
            return "Odds_System_SelAll";
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
