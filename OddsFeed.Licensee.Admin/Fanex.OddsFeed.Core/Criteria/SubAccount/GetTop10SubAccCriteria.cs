// --------------------------------------------------------------------------
// <copyright file="GetTop10SubAccCriteria.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.03.21">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.SubAccount
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class GetTop10SubACCCriteria.
    /// </summary>
    public class GetTop10SubAccCriteria : CriteriaBase
    {
        public GetTop10SubAccCriteria()
        {
        }

        public byte SystemId { get; set; }

        public int RoleId { get; set; }

        public string Username { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_Top10SubAccount_Sel";
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
