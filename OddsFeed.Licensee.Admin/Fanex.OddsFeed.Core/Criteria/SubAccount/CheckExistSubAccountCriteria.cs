// --------------------------------------------------------------------------
// <copyright file="CheckExistSubAccountCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2013.06.26">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.SubAccount
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class CheckExistSubAccountCriteria.
    /// </summary>
    public class CheckExistSubAccountCriteria : CriteriaBase
    {        
        public byte RoleId { get; set; }

        public string Username { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public int RowCount { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_SubAccount_CheckExist";
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
