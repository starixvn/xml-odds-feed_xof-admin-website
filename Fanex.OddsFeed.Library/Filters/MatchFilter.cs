namespace Fanex.OddsFeed.Library.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Nodes;

    public class MatchFilter
    {
        public IList<MatchNode> QueryMatches(OddsProfile oddsProfile, OddsFeedFilter oddsFeedFilter, IEnumerable<MatchNode> matches, bool changeTime, int timeStamp)
        {
            return (from match in matches
                    where (changeTime ? match.TimeStamp > timeStamp : true) &&
                                FilterBySport(oddsProfile, oddsFeedFilter, match) &&
                                FilterByLeague(oddsProfile, oddsFeedFilter, match) &&
                                FilterByCountry(oddsProfile, oddsFeedFilter, match) &&
                                FilterByKeyword(oddsProfile, match)
                    select match).ToList();
        }

        public bool FilterBetType(OddsProfile oddsProfile, OddsFeedFilter oddsFeedFilter, bool isSample, MatchOddsNode matchOdds)
        {
            return FilterByOddsProfile(oddsProfile, isSample, matchOdds)
                && FilterByOddsFeedFilter(oddsFeedFilter, matchOdds);
        }

        private bool FilterByOddsProfile(OddsProfile oddsProfile, bool isSample, MatchOddsNode matchOdds)
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

        private bool FilterByOddsFeedFilter(OddsFeedFilter oddsFeedFilter, MatchOddsNode matchOdds)
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

        private bool FilterBySport(OddsProfile oddsProfile, OddsFeedFilter oddsFeedFilter, MatchNode match)
        {
            return (oddsProfile.Sports == null ? true : (oddsProfile.Sports.Count != 0 ? oddsProfile.Sports.Contains(match.SportId) : false)) &&
                    (oddsFeedFilter.Sports == null || oddsFeedFilter.Sports.Count == 0 ? true : oddsFeedFilter.Sports.Contains(match.SportId));
        }

        private bool FilterByLeague(OddsProfile oddsProfile, OddsFeedFilter oddsFeedFilter, MatchNode match)
        {
            return (oddsProfile.Leagues == null 
                ? true 
                : (oddsProfile.Leagues.Count != 0 ? oddsProfile.Leagues.Contains(match.LeagueId) : false)) 
                && (oddsFeedFilter.Leagues == null || oddsFeedFilter.Leagues.Count == 0 ? true : SearchLeague(oddsFeedFilter.Leagues, match.LeagueNames.ElementAt(0).Content.ToLower()));
        }

        private bool FilterByCountry(OddsProfile oddsProfile, OddsFeedFilter oddsFeedFilter, MatchNode match)
        {
            return (oddsProfile.Countries == null ? true : (oddsProfile.Countries.Count != 0 ? oddsProfile.Countries.Contains(match.CountryCode.ToLower()) : false)) &&
                                         (oddsFeedFilter.Countries == null || oddsFeedFilter.Countries.Count == 0 ? true : oddsFeedFilter.Countries.Contains(match.CountryCode.ToLower()));
        }

        private bool FilterByKeyword(OddsProfile oddsProfile, MatchNode match)
        {
            return (string.IsNullOrEmpty(oddsProfile.LeagueKeyword) ? true : match.LeagueNames.ElementAt(0).Content.ToLower().Contains(oddsProfile.LeagueKeyword.ToLower()));
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
