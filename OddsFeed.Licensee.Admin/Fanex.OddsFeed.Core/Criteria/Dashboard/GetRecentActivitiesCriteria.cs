// --------------------------------------------------------------------------
// <copyright file="GetRecentActivitiesCriteria.cs" company=" Solutions Vietnam">
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
    /// Class GetRecentActivitiesCriteria.
    /// </summary>
    public class GetRecentActivitiesCriteria : CriteriaBase
    {
        public GetRecentActivitiesCriteria()
        {
        }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int RoleID { get; set; }

        [Required]
        public int SystemID { get; set; }


        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public int RowCount { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_Dashboard_RecentActivities";
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
