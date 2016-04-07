// --------------------------------------------------------------------------
// <copyright file="GetOptionalSystemBySubAccountIDCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.10.04">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Administrator
{
    using System.Diagnostics.CodeAnalysis;
    using Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class GetSystemByUserIDCriteria.
    /// </summary>
    public class GetOptionalSystemBySubAccountIDCriteria : CriteriaBase
    {
        public GetOptionalSystemBySubAccountIDCriteria()
        {            
        }

        public int UserID { get; set; }

        public int SubAccountID { get; set; }
        
        public override string GetSettingKey()
        {
            return "Odds_System_SelByUserIDAndSubAccountID";
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
