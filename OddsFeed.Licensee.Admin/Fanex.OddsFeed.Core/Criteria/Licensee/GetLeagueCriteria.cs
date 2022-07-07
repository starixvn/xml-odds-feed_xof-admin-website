namespace Fanex.OddsFeed.Core.Criteria.Licensee
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Data.Repository;

    public class GetLeagueCriteria : CriteriaBase
    {
        public string SportIDList { get; set; }

        public string CountryCodeList { get; set; }

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

        public override string GetSettingKey() => "Odds_LeagueSel";

        public override bool IsValid() => true;
    }
}