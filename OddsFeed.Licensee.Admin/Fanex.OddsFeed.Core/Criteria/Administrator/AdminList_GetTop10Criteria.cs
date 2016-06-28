// --------------------------------------------------------------------------
// <copyright file="AdminList_GetTop10Criteria.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Administrator
{
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class AdminList_GetTop10Criteria.
    /// </summary>    
    public class AdminList_GetTop10Criteria : CriteriaBase
    {
        public AdminList_GetTop10Criteria()
        {
        }

        [Required]
        public byte SystemId { get; set; }

        public string Username { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_AdminList_Top10_Sel";
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
