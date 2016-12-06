namespace Fanex.OddsFeed.WebServiceLibrary.Filters
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.DTO;

    public class OutrightFilter : IOutrightFilter
    {
        public IList<Outright> QueryOutrights(UserProfile userProfile, UserQuery userQuery, IEnumerable<Outright> outrights)
        {
            return (from outright in outrights
                    where
                        FilterBySport(userProfile, userQuery, outright) &&
                        FilterByLeague(userQuery, outright) &&
                        FilterByCountry(userProfile, userQuery, outright) &&
                        FilterByKeyWord(userProfile, outright) &&
                        FilterByProfile(userProfile, outright)
                    select outright).ToList();
        }

        private static bool FilterBySport(UserProfile userProfile, UserQuery userQuery, Outright outright)
        {
            var isProfile = userProfile.Sports == null
                ? true : (userProfile.Sports.Count != 0 ? userProfile.Sports.Contains(outright.SportId) : false);

            var isFilter = userQuery.Sports == null || userQuery.Sports.Count == 0 
                ? true 
                : userQuery.Sports.Contains(outright.SportId);

            return isProfile && isFilter;
        }

        private static bool FilterByLeague(UserQuery userQuery, Outright outright)
        {
            return (userQuery.Leagues == null || userQuery.Leagues.Count == 0 
                ? true 
                : SearchLeague(userQuery.Leagues, outright.EventEnglish.ToLower()));
        }

        private static bool FilterByCountry(UserProfile userProfile, UserQuery userQuery, Outright outright)
        {
            var isProfile = userProfile.Countries == null
                ? true
                : (userProfile.Countries.Count != 0 ? userProfile.Countries.Contains(outright.CountryCode.ToLower()) : false);

            var isFilter = userQuery.Countries == null || userQuery.Countries.Count == 0 
                ? true 
                : userQuery.Countries.Contains(outright.CountryCode.ToLower());


            return isProfile && isFilter;
        }

        private static bool FilterByProfile(UserProfile userProfile, Outright outright)
        {
            return (userProfile.Outrights == null
                ? true
                : (userProfile.Outrights.Count != 0 ? userProfile.Outrights.Contains(outright.EventId) : false));
        }

        private static bool FilterByKeyWord(UserProfile userProfile, Outright outright)
        {
            return string.IsNullOrEmpty(userProfile.OutrightsKeyword) 
                ? true 
                : outright.EventEnglish.ToLower().Contains(userProfile.OutrightsKeyword.ToLower());
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
