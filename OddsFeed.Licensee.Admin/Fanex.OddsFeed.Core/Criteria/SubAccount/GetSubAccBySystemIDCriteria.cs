// --------------------------------------------------------------------------
// <copyright file="GetSubAccBySystemIDCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using System.Diagnostics.CodeAnalysis;
    using Framework.Data;

    /// <summary>
    /// Class GetSubACCBySystemIDCriteria.
    /// </summary>
    public class GetSubAccBySystemIDCriteria : CriteriaBase
    {
        public GetSubAccBySystemIDCriteria()
        {            
        }

        public byte SystemId { get; set; }

        public byte RoleId { get; set; }
        
        public override string GetSettingKey()
        {
            return "Odds_User_GetBySystemIdAndRoleId";
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
