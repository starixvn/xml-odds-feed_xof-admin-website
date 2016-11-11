namespace Fanex.OddsFeed.Library.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Nodes;

    public class OutrightFilter
    {
        public IList<OutrightNode> QueryOutrights(OddsProfile oddsProfile, OddsFeedFilter oddsFeedFilter, IEnumerable<OutrightNode> outrights)
        {
            return (from outright in outrights
                    where
                        FilterBySport(oddsProfile, oddsFeedFilter, outright) &&
                        FilterByLeague(oddsFeedFilter, outright) &&
                        FilterByCountry(oddsProfile, oddsFeedFilter, outright) &&
                        FilterByKeyWord(oddsProfile, outright) &&
                        FilterByProfile(oddsProfile, outright)
                    select outright).ToList();
        }

        private bool FilterBySport(OddsProfile oddsProfile, OddsFeedFilter oddsFeedFilter, OutrightNode outright)
        {
            var isProfile = oddsProfile.Sports == null
                ? true : (oddsProfile.Sports.Count != 0 ? oddsProfile.Sports.Contains(outright.SportId) : false);

            var isFilter = oddsFeedFilter.Sports == null || oddsFeedFilter.Sports.Count == 0 
                ? true 
                : oddsFeedFilter.Sports.Contains(outright.SportId);

            return isProfile && isFilter;
        }

        private bool FilterByLeague(OddsFeedFilter oddsFeedFilter, OutrightNode outright)
        {
            return (oddsFeedFilter.Leagues == null || oddsFeedFilter.Leagues.Count == 0 
                ? true 
                : SearchLeague(oddsFeedFilter.Leagues, outright.EventNames.ElementAt(0).Content.ToLower()));
        }

        private bool FilterByCountry(OddsProfile oddsProfile, OddsFeedFilter oddsFeedFilter, OutrightNode outright)
        {
            var isProfile = oddsProfile.Countries == null
                ? true
                : (oddsProfile.Countries.Count != 0 ? oddsProfile.Countries.Contains(outright.CountryCode.ToLower()) : false);

            var isFilter = oddsFeedFilter.Countries == null || oddsFeedFilter.Countries.Count == 0 
                ? true 
                : oddsFeedFilter.Countries.Contains(outright.CountryCode.ToLower());


            return isProfile && isFilter;
        }

        private bool FilterByProfile(OddsProfile oddsProfile, OutrightNode outright)
        {
            return (oddsProfile.Outrights == null
                ? true
                : (oddsProfile.Outrights.Count != 0 ? oddsProfile.Outrights.Contains(outright.EventId) : false));
        }

        private bool FilterByKeyWord(OddsProfile oddsProfile, OutrightNode outright)
        {
            return string.IsNullOrEmpty(oddsProfile.OutrightsKeyword) 
                ? true 
                : outright.EventNames.ElementAt(0).Content.ToLower().Contains(oddsProfile.OutrightsKeyword.ToLower());
        }

        private bool SearchLeague(IList<string> searchLeagues, string league)
        {
            foreach (string item in searchLeagues)
            {
                if (league.Contains(item))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
