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
            return (userProfile.Sports == null || userProfile.Sports.Contains(outright.SportId))
                && (userQuery.Sports == null || userQuery.Sports.Contains(outright.SportId));
        }

        private static bool FilterByLeague(UserQuery userQuery, Outright outright)
        {
            return userQuery.Leagues == null || SearchLeague(userQuery.Leagues, outright.EventEnglish.ToLower());
        }

        private static bool FilterByCountry(UserProfile userProfile, UserQuery userQuery, Outright outright)
        {
            return (userProfile.Countries == null || userProfile.Countries.Contains(outright.CountryCode.ToLower()))
                && (userQuery.Countries == null || userQuery.Countries.Contains(outright.CountryCode.ToLower()));
        }

        private static bool FilterByProfile(UserProfile userProfile, Outright outright)
        {
            return userProfile.Outrights == null || userProfile.Outrights.Contains(outright.EventId);
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
