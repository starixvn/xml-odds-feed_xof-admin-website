// --------------------------------------------------------------------------
// <copyright file="GetLicenseeByPageCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.2.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Licensee
{
    using Data.Repository;
    using Fanex.OddsFeed.Framework.Data;

    /// <summary>
    /// Class GetLicenseeByPageCriteria.
    /// </summary>
    public class GetLicenseeByPageCriteria : CriteriaBase
    {
        public string SystemName { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public int RowCount { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_LicenseeSelByPage";
        }
    }
}