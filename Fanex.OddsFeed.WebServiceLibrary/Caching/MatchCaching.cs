namespace Fanex.OddsFeed.WebServiceLibrary.Caching
{
    using System.Collections.Generic;
    using System.Linq;
    using Couchbase.Core;
    using Fanex.OddsFeed.Common.DTO;
    using Fanex.OddsFeed.WebServiceLibrary.Models;

    public class MatchCaching : IMatchCaching
    {
        private readonly string _matchBucket;
        private readonly IVersionCaching _versionCaching;

        public MatchCaching(string matchBucket)
        {
            _matchBucket = matchBucket;
            _versionCaching = new VersionCaching();
        }

        public SafeList<Match> GetMatches(ICluster cluster, OddsFeedsModel model, int cacheVersion)
        {
            var data = new List<Match>();
            using (var bucket = cluster.OpenBucket(_matchBucket))
            {
                var query = BuildQuery(model, cacheVersion);
                data = bucket.Query<Match>(query).ToList();
            }

            if (data.Count == 1 && data.First().MatchOddsList == null)
            {
                data = new List<Match> { };
            }

            return new SafeList<Match>(data);
        }

        private string BuildQuery(OddsFeedsModel model, int cacheVersion)
        {
            var matches = "matches";
            var matchOddsList = "matchOddsList";
            var queryTemplate = @"select matches.matchId, matches.leagueId, matches.leagueEn, matches.leagueTw, matches.leagueCn, 
                            matches.homeEn, matches.homeTw, matches.homeCn, matches.awayEn, matches.awayTw, matches.awayCn, 
                            matches.sportId, matches.sportName, matches.countryCode, matches.kickOfTime, 
                            ARRAY_AGG(matchOddsList) as matchOddsList
                        from {0} unnest {1}.matches as {2} unnest matches.matchOddsList as {3} 
                        {4}
                        group by matches.matchId, matches.leagueId, matches.leagueEn, matches.leagueTw, matches.leagueCn, 
                                matches.homeEn, matches.homeTw, matches.homeCn, matches.awayEn, matches.awayTw, matches.awayCn, 
                                matches.sportId, matches.sportName, matches.countryCode, matches.kickOfTime";
            var conditions = new[] {
                BuildFilterByVersion(_matchBucket, cacheVersion),
                BuildFilterBySport(model.UserProfile, model.UserQuery, matches),
                BuildFilterByUserProfileLeague(model.UserProfile, matches),
                BuildFilterByUserQueryLeague(model.UserQuery, matches),
                BuildFilterByCountry(model.UserProfile, model.UserQuery, matches),
                BuildFilterByKeyword(model.UserProfile, matches),
                BuildFilterByBetType(model.UserProfile, model.UserQuery, matchOddsList),
                BuildFilterByTimeStamp(model.TimeStamp, matchOddsList)
            }.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            var whereClause = conditions.Any() ? $" where {string.Join(" and ", conditions)} " : string.Empty;

            return string.Format(queryTemplate, _matchBucket, _matchBucket, matches, matchOddsList, whereClause);
        }

        private string BuildFilterByVersion(string matchBucket, int cacheVersion)
        {
            return $" {matchBucket}.version = {cacheVersion} ";
        }

        private string BuildFilterBySport(UserProfile userProfile, UserQuery userQuery, string matches)
        {
            var sportIds = new List<int>();

            if (userProfile.Sports != null && userProfile.Sports.Any())
            {
                sportIds.AddRange(userProfile.Sports);
            }

            if (userQuery.Sports != null && userQuery.Sports.Any())
            {
                sportIds.AddRange(userQuery.Sports);
            }

            return sportIds.Any() ? $" {matches}.sportId in [{string.Join(",", sportIds)}] " : string.Empty;
        }

        private string BuildFilterByUserProfileLeague(UserProfile userProfile, string matches)
        {
            var leagues = new List<int>();

            if (userProfile.Leagues != null && userProfile.Leagues.Any())
            {
                leagues.AddRange(userProfile.Leagues);
            }

            return leagues.Any() ? $" lower({matches}.leagueId) in [{string.Join(",", leagues)}] " : string.Empty;
        }

        private string BuildFilterByUserQueryLeague(UserQuery userQuery, string matches)
        {
            var leagues = new List<string>();

            if (userQuery.Leagues != null && userQuery.Leagues.Any())
            {
                leagues.AddRange(userQuery.Leagues);
            }

            leagues = leagues.Where(l => l != null).Select(l => { return "'" + l.ToLower() + "'"; }).ToList();

            return leagues.Any() ? $" lower({matches}.leagueEn) in [{string.Join(",", leagues)}] " : string.Empty;
        }

        private string BuildFilterByCountry(UserProfile userProfile, UserQuery userQuery, string matches)
        {
            var countries = new List<string>();

            if (userProfile.Countries != null && userProfile.Countries.Any())
            {
                countries.AddRange(userProfile.Countries);
            }
            if (userQuery.Countries != null && userQuery.Countries.Any())
            {
                countries.AddRange(userQuery.Countries);
            }

            countries = countries.Where(c => c != null).Select(c => { return "'" + c.ToLower() + "'"; }).ToList();

            return countries.Any() ? $" lower({matches}.countryCode) in [{string.Join(",", countries)}] " : string.Empty;
        }

        private string BuildFilterByKeyword(UserProfile userProfile, string matches)
        {
            return string.IsNullOrEmpty(userProfile.LeagueKeyword) ? string.Empty : $" lower({matches}.leagueEn) like '%{userProfile.LeagueKeyword.ToLower()}%' ";
        }

        private string BuildFilterByBetType(UserProfile userProfile, UserQuery userQuery, string matchOddsList)
        {
            var bettypeIds = new List<int>();
            if (userProfile.BetTypes != null && userProfile.BetTypes.Any())
            {
                bettypeIds.AddRange(userProfile.BetTypes.ToList());
            }

            if (userQuery.BetTypes != null && userQuery.BetTypes.Any())
            {
                bettypeIds.AddRange(userQuery.BetTypes.ToList());
            }

            return bettypeIds.Any() ? $" {matchOddsList}.betTypeId in [{string.Join(", ", bettypeIds)}] " : string.Empty;
        }

        private string BuildFilterByTimeStamp(string timeStamp, string matchOddsList)
        {
            var changeTime = !string.IsNullOrEmpty(timeStamp);
            var _timeStamp = changeTime ? int.Parse(timeStamp) : 0;

            return changeTime ? $" {matchOddsList}.timeStamp > {_timeStamp} " : string.Empty;
        }
    }
}
