// --------------------------------------------------------------------------
// <copyright file="CheckExistLicenseeCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2013.06.26">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Licensee
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Framework.Data;

    /// <summary>
    /// Class CheckExistLicenseeCriteria.
    /// </summary>    
    public class CheckExistLicenseeCriteria : CriteriaBase
    {
        public string SystemName { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public int RowCount { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_Licensee_CheckExistLicensee";
        }
    }
}
