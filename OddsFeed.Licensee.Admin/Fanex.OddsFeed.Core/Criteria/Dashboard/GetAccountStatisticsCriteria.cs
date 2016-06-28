// --------------------------------------------------------------------------
// <copyright file="GetAccountStatisticsCriteria.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
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
    /// Class GetAccountStatisticsCriteria.
    /// </summary>
    public class GetAccountStatisticsCriteria : CriteriaBase
    {
        public GetAccountStatisticsCriteria()
        {
        }

        [Required]
        public int SystemID { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_Dashboard_AccountStatistics";
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
