namespace Fanex.OddsFeed.WebServiceLibrary.Filters
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.DTO;
    using Models;

    public class OutrightFilter : IOutrightFilter
    {
        public IList<Outright> QueryOutrights(IEnumerable<Outright> outrights, UserProfile userProfile, UserQuery userQuery)
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
            return (userProfile.Sports == null || userProfile.Sports.Count == 0 || userProfile.Sports.Contains(outright.SportId))
                && (userQuery.Sports == null || userQuery.Sports.Count == 0 || userQuery.Sports.Contains(outright.SportId));
        }

        private static bool FilterByLeague(UserQuery userQuery, Outright outright)
        {
            return userQuery.Leagues == null || userQuery.Leagues.Count == 0 || SearchLeague(userQuery.Leagues, outright.EventEnglish.ToLower());
        }

        private static bool FilterByCountry(UserProfile userProfile, UserQuery userQuery, Outright outright)
        {
            return (userProfile.Countries == null || userProfile.Countries.Count == 0 || userProfile.Countries.Contains(outright.CountryCode.ToLower()))
                && (userQuery.Countries == null || userQuery.Countries.Count == 0 || userQuery.Countries.Contains(outright.CountryCode.ToLower()));
        }

        private static bool FilterByProfile(UserProfile userProfile, Outright outright)
        {
            return userProfile.Outrights == null || userProfile.Outrights.Count ==0 || userProfile.Outrights.Contains(outright.EventId);
        }

        private static bool FilterByKeyWord(UserProfile userProfile, Outright outright)
        {
            return string.IsNullOrEmpty(userProfile.OutrightsKeyword) 
                || outright.EventEnglish.ToLower().Contains(userProfile.OutrightsKeyword.ToLower());
        }

        private static bool SearchLeague(IList<string> searchLeagues, string league)
        {
            return searchLeagues.Any(i => league.Contains(i));
        }
    }
}
