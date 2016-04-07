// --------------------------------------------------------------------------
// <copyright file="GetLoginInfoCriteria.cs" company="Nexcel Solutions Vietnam">
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
    using Data.Repository;
    /// <summary>
    /// Class GetLoginInfoCriteria.
    /// </summary>
    public class GetLoginInfoCriteria : CriteriaBase
    {
        public GetLoginInfoCriteria()
        {
        }

        [Required]
        public int UserID { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public int RowCount { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_Dashboard_LastLogin";
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
