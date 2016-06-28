// --------------------------------------------------------------------------
// <copyright file="GetOutRightCriteria.cs" company=" Solutions Vietnam">
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
    /// Class GetOutRightCriteria.
    /// </summary>
    public class GetOutRightCriteria : CriteriaBase
    {
        public string SportIDList { get; set; }

        public string CountryCodeList { get; set; }


        // Ignored
        [SpParam(Ignored = true)]
        public string OutRightIDList { get; set; }

        [SpParam(Ignored = true)]
        public string LeagueKeyword_O { get; set; }

        [SpParam(Ignored = true)]
        public bool LeagueKeyword_OFlag { get; set; }

        [SpParam(Ignored = true)]
        public List<SelectListItem> ViewBagOutrightList { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_OutRightsSel";
        }
    }
}