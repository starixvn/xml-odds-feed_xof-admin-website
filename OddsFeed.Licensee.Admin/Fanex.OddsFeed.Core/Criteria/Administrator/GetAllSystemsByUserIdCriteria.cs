// --------------------------------------------------------------------------
// <copyright file="GetAllSystemsByUserIdCriteria.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2013.03.06">Created</change>
//   <change who="Thai.Tran" date="2014.05.20">Updated remove "SystemId"</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Administrator
{
    using System.Diagnostics.CodeAnalysis;
    using Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class GetAllSystemsByUserIdCriteria.
    /// </summary>    
    public class GetAllSystemsByUserIdCriteria : CriteriaBase
    {
        public GetAllSystemsByUserIdCriteria()
        {
        }

        public byte UserRole { get; set; }

        public int UserId { get; set; }
        
        public override string GetSettingKey()
        {
            return "Odds_AdminSystem_SelAll";
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
