namespace Fanex.OddsFeed.Library.Filters
{
    using System.Collections.Generic;
    using System.Linq;
    using Nodes;

    public class MatchFilter : IMatchFilter
    {
        public IList<Match> QueryMatches(OddsProfile oddsProfile, OddsFeedFilter oddsFeedFilter, IList<Match> matches, bool changeTime, int timeStamp)
        {
            return (from match in matches
                    where (changeTime ? match.TimeStamp > timeStamp : true) &&
                                FilterBySport(oddsProfile, oddsFeedFilter, match) &&
                                FilterByLeague(oddsProfile, oddsFeedFilter, match) &&
                                FilterByCountry(oddsProfile, oddsFeedFilter, match) &&
                                FilterByKeyword(oddsProfile, match)
                    select match).ToList();
        }

        public bool FilterBetType(OddsProfile oddsProfile, OddsFeedFilter oddsFeedFilter, bool isSample, MatchOdds matchOdds)
        {
            return FilterByOddsProfile(oddsProfile, isSample, matchOdds)
                && FilterByOddsFeedFilter(oddsFeedFilter, matchOdds);
        }

        private static bool FilterByOddsProfile(OddsProfile oddsProfile, bool isSample, MatchOdds matchOdds)
        {
            if (oddsProfile.BetTypes != null && !isSample)
            {
                if (!oddsProfile.BetTypes.Contains(matchOdds.BetTypeId))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool FilterByOddsFeedFilter(OddsFeedFilter oddsFeedFilter, MatchOdds matchOdds)
        {
            if (oddsFeedFilter.BetTypes != null && oddsFeedFilter.BetTypes.Count > 0)
            {
                if (!oddsFeedFilter.BetTypes.Contains(matchOdds.BetTypeId))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool FilterBySport(OddsProfile oddsProfile, OddsFeedFilter oddsFeedFilter, Match match)
        {
            return (oddsProfile.Sports == null ? true : (oddsProfile.Sports.Count != 0 ? oddsProfile.Sports.Contains(match.SportId) : false)) &&
                    (oddsFeedFilter.Sports == null || oddsFeedFilter.Sports.Count == 0 ? true : oddsFeedFilter.Sports.Contains(match.SportId));
        }

        private static bool FilterByLeague(OddsProfile oddsProfile, OddsFeedFilter oddsFeedFilter, Match match)
        {
            return (oddsProfile.Leagues == null 
                ? true 
                : (oddsProfile.Leagues.Count != 0 ? oddsProfile.Leagues.Contains(match.LeagueId) : false)) 
                && (oddsFeedFilter.Leagues == null || oddsFeedFilter.Leagues.Count == 0 ? true : SearchLeague(oddsFeedFilter.Leagues, match.LeagueNames.ElementAt(0).Content.ToLower()));
        }

        private static bool FilterByCountry(OddsProfile oddsProfile, OddsFeedFilter oddsFeedFilter, Match match)
        {
            return (oddsProfile.Countries == null ? true : (oddsProfile.Countries.Count != 0 ? oddsProfile.Countries.Contains(match.CountryCode.ToLower()) : false)) &&
                                         (oddsFeedFilter.Countries == null || oddsFeedFilter.Countries.Count == 0 ? true : oddsFeedFilter.Countries.Contains(match.CountryCode.ToLower()));
        }

        private static bool FilterByKeyword(OddsProfile oddsProfile, Match match)
        {
            return (string.IsNullOrEmpty(oddsProfile.LeagueKeyword) ? true : match.LeagueNames.ElementAt(0).Content.ToLower().Contains(oddsProfile.LeagueKeyword.ToLower()));
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
