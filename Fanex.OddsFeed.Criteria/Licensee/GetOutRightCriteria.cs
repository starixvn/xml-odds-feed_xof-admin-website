namespace Fanex.OddsFeed.Criteria.Licensee
{
    using System.Collections.Generic;
    using Data.Repository;
    using Fanex.Data;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class GetOutRightCriteria : CriteriaBase
    {
        public string SportIDList { get; set; }

        public string CountryCodeList { get; set; }

        [SpParam(Ignored = true)]
        public string OutRightIDList { get; set; }

        [SpParam(Ignored = true)]
        public string LeagueKeyword_O { get; set; }

        [SpParam(Ignored = true)]
        public bool LeagueKeyword_OFlag { get; set; }

        [SpParam(Ignored = true)]
        public List<SelectListItem> ViewBagOutrightList { get; set; }

        public override string GetSettingKey() => "Odds_OutRightsSel";

        public override bool IsValid() => true;
    }
}