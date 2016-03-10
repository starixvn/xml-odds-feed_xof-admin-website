// --------------------------------------------------------------------------
// <copyright file="GetSystemByUserIDCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2013.03.06">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Administrator
{
    using System.Diagnostics.CodeAnalysis;
    using Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class GetSystemByUserIDCriteria.
    /// </summary>
    public class GetSystemByUserIDCriteria : CriteriaBase
    {
        public GetSystemByUserIDCriteria()
        {            
        }

        public int UserID { get; set; }

        public bool IgnoreIsActive { get; set; }
        
        public override string GetSettingKey()
        {
            return "Odds_System_SelByUserID";
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
