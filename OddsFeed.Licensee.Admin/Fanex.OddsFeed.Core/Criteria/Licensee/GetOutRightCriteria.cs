// --------------------------------------------------------------------------
// <copyright file="GetOutRightCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Class GetOutRightCriteria.
    /// </summary>
    public class GetOutRightCriteria : CriteriaBase
    {
        public string SportIDList { get; set; }

        public string CountryCodeList { get; set; }

        public string OutRightIDList { get; set; }

        public string LeagueKeyword_O { get; set; }

        public bool LeagueKeyword_OFlag { get; set; }

        public List<SelectListItem> ViewBagOutrightList { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_OutRightsSel";
        }
    }
}