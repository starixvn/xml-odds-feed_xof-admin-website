// --------------------------------------------------------------------------
// <copyright file="GetSubAccByNameCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.02.18">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.SubAccount
{
    using System.Diagnostics.CodeAnalysis;
    using Core.Business;
    using Framework.Data;

    /// <summary>
    /// Class GetSubACCByNameCriteria.
    /// </summary>
    public class GetSubAccByNameCriteria : CriteriaBase
    {
        public GetSubAccByNameCriteria()
        {            
        }

        public byte SystemId { get; set; }

        public byte RoleId { get; set; }

        public string Username { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public int RowCount { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_User_SelByName_SystemId_RoleId";
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
