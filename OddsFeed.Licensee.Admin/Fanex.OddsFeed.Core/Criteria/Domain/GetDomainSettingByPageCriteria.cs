// --------------------------------------------------------------------------
// <copyright file="GetDomainSettingByPageCriteria.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.02.22">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Domain
{
    using System;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class GetDomainSettingByPageCriteria.
    /// </summary>
    public class GetDomainSettingByPageCriteria : CriteriaBase
    {
        public short DomainID { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public int RowCount { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_DomainSettingSelByPage";
        }
    }
}
