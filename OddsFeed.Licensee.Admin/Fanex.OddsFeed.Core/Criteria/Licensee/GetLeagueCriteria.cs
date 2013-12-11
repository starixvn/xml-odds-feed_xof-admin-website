// --------------------------------------------------------------------------
// <copyright file="GetLeagueCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
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

    /// <summary>
    /// Class GetLeagueCriteria.
    /// </summary>
    public class GetLeagueCriteria : CriteriaBase
    {
        public string SportIDList { get; set; }

        public string CountryCodeList { get; set; }

        public string LeagueIDList { get; set; }

        public string LeagueKeyword { get; set; }

        public bool LeagueKeywordFlag { get; set; }

        public bool LeagueIDListFlag { get; set; }

        public List<SelectListItem> ViewBagLeagueList { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_LeagueSel";
        }
    }
}