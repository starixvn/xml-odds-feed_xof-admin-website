// --------------------------------------------------------------------------
// <copyright file="GetCurrentOnlineUserCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2013.03.05">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Dashboard
{
    using System.ComponentModel.DataAnnotations;
    using Framework.Data;

    /// <summary>
    /// Class GetCurrentOnlineUserCriteria.
    /// </summary>
    public class GetCurrentOnlineUserCriteria : CriteriaBase
    {
        public GetCurrentOnlineUserCriteria()
        {
        }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int RoleID { get; set; }

        [Required]
        public int SystemID { get; set; }
        
        public override string GetSettingKey()
        {
            return "Odds_Dashboard_CurrentOnlUsers";
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
