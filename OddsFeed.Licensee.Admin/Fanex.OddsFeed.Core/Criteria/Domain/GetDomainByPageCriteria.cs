// --------------------------------------------------------------------------
// <copyright file="GetDomainByPageCriteria.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.02.22">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Domain
{
    using Data.Repository;
    using Fanex.OddsFeed.Framework.Data;

    /// <summary>
    /// Class GetDomainByPageCriteria.
    /// </summary>
    public class GetDomainByPageCriteria : CriteriaBase
    {
        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public int RowCount { get; set; }

        public string DomainName { get; set; }


        // Ignored
        [SpParam(Ignored = true)]
        public int DomainId { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_DomainSelByPage";
        }
    }
}
