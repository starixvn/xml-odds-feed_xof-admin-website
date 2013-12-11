// --------------------------------------------------------------------------
// <copyright file="GetAllSystemCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2013.03.06">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Administrator
{
    using System.Diagnostics.CodeAnalysis;
    using Framework.Data;

    /// <summary>
    /// Class GetAllSystemCriteria.
    /// </summary>    
    public class GetAllSystemCriteria : CriteriaBase
    {      
        public GetAllSystemCriteria()
        {
        }

        public byte UserRole { get; set; }

        public int UserId { get; set; }

        public int SystemId { get; set; }     
        
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
