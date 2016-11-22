namespace Fanex.OddsFeed.Library.Filters
{
    using System.Collections.Generic;
    using System.Linq;
    using Nodes;

    public class OutrightFilter
    {
        public IList<Outright> QueryOutrights(OddsProfile oddsProfile, OddsFeedFilter oddsFeedFilter, IEnumerable<Outright> outrights)
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

        private static bool FilterBySport(OddsProfile oddsProfile, OddsFeedFilter oddsFeedFilter, Outright outright)
        {
            var isProfile = oddsProfile.Sports == null
                ? true : (oddsProfile.Sports.Count != 0 ? oddsProfile.Sports.Contains(outright.SportId) : false);

            var isFilter = oddsFeedFilter.Sports == null || oddsFeedFilter.Sports.Count == 0 
                ? true 
                : oddsFeedFilter.Sports.Contains(outright.SportId);

            return isProfile && isFilter;
        }

        private static bool FilterByLeague(OddsFeedFilter oddsFeedFilter, Outright outright)
        {
            return (oddsFeedFilter.Leagues == null || oddsFeedFilter.Leagues.Count == 0 
                ? true 
                : SearchLeague(oddsFeedFilter.Leagues, outright.EventNames.ElementAt(0).Content.ToLower()));
        }

        private static bool FilterByCountry(OddsProfile oddsProfile, OddsFeedFilter oddsFeedFilter, Outright outright)
        {
            var isProfile = oddsProfile.Countries == null
                ? true
                : (oddsProfile.Countries.Count != 0 ? oddsProfile.Countries.Contains(outright.CountryCode.ToLower()) : false);

            var isFilter = oddsFeedFilter.Countries == null || oddsFeedFilter.Countries.Count == 0 
                ? true 
                : oddsFeedFilter.Countries.Contains(outright.CountryCode.ToLower());


            return isProfile && isFilter;
        }

        private static bool FilterByProfile(OddsProfile oddsProfile, Outright outright)
        {
            return (oddsProfile.Outrights == null
                ? true
                : (oddsProfile.Outrights.Count != 0 ? oddsProfile.Outrights.Contains(outright.EventId) : false));
        }

        private static bool FilterByKeyWord(OddsProfile oddsProfile, Outright outright)
        {
            return string.IsNullOrEmpty(oddsProfile.OutrightsKeyword) 
                ? true 
                : outright.EventNames.ElementAt(0).Content.ToLower().Contains(oddsProfile.OutrightsKeyword.ToLower());
        }

        private static bool SearchLeague(IList<string> searchLeagues, string league)
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
