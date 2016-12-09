namespace Fanex.OddsFeed.WebServiceLibrary.Filters
{
    using Common.DTO;
    using System.Collections.Generic;
    using System.Linq;

    public class MatchFilter : IMatchFilter
    {
        public IList<Match> QueryMatches(IList<Match> matches, UserProfile userProfile, UserQuery userQuery,  bool changeTime, int timeStamp)
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
            return isSample || userProfile.BetTypes == null || userProfile.BetTypes.Contains(matchOdds.BetTypeId);
        }

        private static bool FilterByOddsFeedFilter(UserQuery userQuery, MatchOdds matchOdds)
        {
            return userQuery.BetTypes == null || userQuery.BetTypes.Contains(matchOdds.BetTypeId);
        }

        private static bool FilterBySport(UserProfile userProfile, UserQuery userQuery, Match match)
        {
            return (userProfile.Sports == null || userProfile.Sports.Contains(match.SportId)) 
                && (userQuery.Sports == null || userQuery.Sports.Contains(match.SportId));
        }

        private static bool FilterByLeague(UserProfile userProfile, UserQuery userQuery, Match match)
        {
            return (userProfile.Leagues == null || userProfile.Leagues.Contains(match.LeagueId))
                && (userQuery.Leagues == null || SearchLeague(userQuery.Leagues, match.LeagueEnglish.ToLower()));
        }

        private static bool FilterByCountry(UserProfile userProfile, UserQuery userQuery, Match match)
        {
            return (userProfile.Countries == null || userProfile.Countries.Contains(match.CountryCode.ToLower()))
                && (userQuery.Countries == null || userQuery.Countries.Contains(match.CountryCode.ToLower()));
        }

        private static bool FilterByKeyword(UserProfile userProfile, Match match)
        {
            return (string.IsNullOrEmpty(userProfile.LeagueKeyword) ? true : match.LeagueEnglish.ToLower().Contains(userProfile.LeagueKeyword.ToLower()));
        }

        private static bool SearchLeague(IList<string> searchLeagues, string league)
        {
            return searchLeagues.Any(keyword => league.Contains(keyword));
        }
    }
}