// --------------------------------------------------------------------------
// <copyright file="GetLeagueCriteria.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.3.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Licensee
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class GetLeagueCriteria.
    /// </summary>
    public class GetLeagueCriteria : CriteriaBase
    {
        public string SportIDList { get; set; }

        public string CountryCodeList { get; set; }


        // Ignored
        [SpParam(Ignored = true)]
        public string LeagueIDList { get; set; }

        [SpParam(Ignored = true)]
        public string LeagueKeyword { get; set; }

        [SpParam(Ignored = true)]
        public bool LeagueKeywordFlag { get; set; }

        [SpParam(Ignored = true)]
        public bool LeagueIDListFlag { get; set; }

        [SpParam(Ignored = true)]
        public List<SelectListItem> ViewBagLeagueList { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_LeagueSel";
        }
    }
}