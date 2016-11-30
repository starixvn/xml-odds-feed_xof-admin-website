namespace Fanex.OddsFeed.WebServiceLibrary.Filters
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.DTO;

    public class MatchFilter : IMatchFilter
    {
        public IList<Match> QueryMatches(UserProfile userProfile, UserQuery userQuery, IList<Match> matches, bool changeTime, int timeStamp)
        {
            return (from match in matches
                    where (changeTime ? match.TimeStamp > timeStamp : true) &&
                                FilterBySport(userProfile, userQuery, match) &&
                                FilterByLeague(userProfile, userQuery, match) &&
                                FilterByCountry(userProfile, userQuery, match) &&
                                FilterByKeyword(userProfile, match)
                    select match).ToList();
        }

        public bool FilterBetType(UserProfile userProfile, UserQuery userQuery, bool isSample, MatchOdds matchOdds)
        {
            return FilterByOddsProfile(userProfile, isSample, matchOdds)
                && FilterByOddsFeedFilter(userQuery, matchOdds);
        }

        private static bool FilterByOddsProfile(UserProfile userProfile, bool isSample, MatchOdds matchOdds)
        {
            if (userProfile.BetTypes != null && !isSample)
            {
                if (!userProfile.BetTypes.Contains(matchOdds.BetTypeId))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool FilterByOddsFeedFilter(UserQuery userQuery, MatchOdds matchOdds)
        {
            if (userQuery.BetTypes != null && userQuery.BetTypes.Count > 0)
            {
                if (!userQuery.BetTypes.Contains(matchOdds.BetTypeId))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool FilterBySport(UserProfile userProfile, UserQuery userQuery, Match match)
        {
            return (userProfile.Sports == null ? true : (userProfile.Sports.Count != 0 ? userProfile.Sports.Contains(match.SportId) : false)) &&
                    (userQuery.Sports == null || userQuery.Sports.Count == 0 ? true : userQuery.Sports.Contains(match.SportId));
        }

        private static bool FilterByLeague(UserProfile userProfile, UserQuery userQuery, Match match)
        {
            return (userProfile.Leagues == null 
                ? true 
                : (userProfile.Leagues.Count != 0 ? userProfile.Leagues.Contains(match.LeagueId) : false)) 
                && (userQuery.Leagues == null || userQuery.Leagues.Count == 0 ? true : SearchLeague(userQuery.Leagues, match.LeagueNames.ElementAt(0).Content.ToLower()));
        }

        private static bool FilterByCountry(UserProfile userProfile, UserQuery userQuery, Match match)
        {
            return (userProfile.Countries == null ? true : (userProfile.Countries.Count != 0 ? userProfile.Countries.Contains(match.CountryCode.ToLower()) : false)) &&
                                         (userQuery.Countries == null || userQuery.Countries.Count == 0 ? true : userQuery.Countries.Contains(match.CountryCode.ToLower()));
        }

        private static bool FilterByKeyword(UserProfile userProfile, Match match)
        {
            return (string.IsNullOrEmpty(userProfile.LeagueKeyword) ? true : match.LeagueNames.ElementAt(0).Content.ToLower().Contains(userProfile.LeagueKeyword.ToLower()));
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
