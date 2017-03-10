using System.Collections.Generic;
using System.Linq;
using Couchbase.Core;
using Fanex.OddsFeed.Common.Configurations;
using Fanex.OddsFeed.Common.DTO;
using Fanex.OddsFeed.WebServiceLibrary.Models;

namespace Fanex.OddsFeed.WebServiceLibrary.Caching
{
    public class MatchCaching : IMatchCaching
    {
        private readonly string _matchBucket;
        private readonly bool _isLive;
        private readonly IVersionCaching _versionCaching;

        public MatchCaching(bool isLive)
        {
            _isLive = isLive;
            _matchBucket = isLive ? AppSettings.BucketLiveMatches : AppSettings.BucketDeadMatches;
            _versionCaching = new VersionCaching();
        }

        public SafeList<Match> GetMatches(ICluster cluster, OddsFeedsModel model, CacheVersion cacheVersion)
        {
            var data = new List<Match> { };
            using (var bucket = cluster.OpenBucket(_matchBucket))
            {
                var query = BuildQuery(model, cacheVersion);
                data = bucket.Query<Match>(query).ToList();
            }

            return new SafeList<Match>(data);
        }

        private string BuildQuery(OddsFeedsModel model, CacheVersion cacheVersion)
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
            var whereClause = conditions.Any() ? string.Format(" where {0} ", string.Join(" and ", conditions)) : string.Empty;

            return string.Format(queryTemplate, _matchBucket, _matchBucket, matches, matchOddsList, whereClause);
        }

        private string BuildFilterByVersion(string matchBucket, CacheVersion cacheVersion)
        {
            var version = _isLive ? cacheVersion.CurrentVersionLiveMatch : cacheVersion.CurrentVersionDeadballMatch;

            return string.Format(" {0}.version = {1} ", matchBucket, version);
        }

        private string BuildFilterBySport(UserProfile userProfile, UserQuery userQuery, string matches)
        {
            var sportIds = new List<int>();
            if (userProfile.Sports != null && userProfile.Sports.Any())
            {
                sportIds.AddRange(userProfile.Sports.ToList());
            }

            if (userQuery.Sports != null && userQuery.Sports.Any())
            {
                sportIds.AddRange(userQuery.Sports.ToList());
            }

            return sportIds.Any() ? string.Format(" {0}.sportId in [{1}] ", matches, string.Join(",", sportIds))
                : string.Empty;
        }

        private string BuildFilterByUserProfileLeague(UserProfile userProfile, string matches)
        {
            var leagues = new List<int>();
            if (userProfile.Leagues != null && userProfile.Leagues.Any())
            {
                leagues.AddRange(userProfile.Leagues);
            }

            return leagues.Any() ? string.Format(" lower({0}.leagueId) in [{1}] ", matches, string.Join(",", leagues))
                : string.Empty;
        }

        private string BuildFilterByUserQueryLeague(UserQuery userQuery, string matches)
        {
            var leagues = new List<string>();
            if (userQuery.Leagues != null && userQuery.Leagues.Any())
            {
                leagues.AddRange(userQuery.Leagues);
            }
            leagues = leagues.Where(l => l != null).Select(l => { return "'" + l.ToLower() + "'"; }).ToList();

            return leagues.Any() ? string.Format(" lower({0}.leagueEn) in [{1}] ", matches, string.Join(",", leagues))
                : string.Empty;
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

            return countries.Any() ? string.Format(" lower({0}.countryCode) in [{1}] ", matches, string.Join(",", countries))
                : string.Empty;
        }

        private string BuildFilterByKeyword(UserProfile userProfile, string matches)
        {
            return string.IsNullOrEmpty(userProfile.LeagueKeyword) ? string.Empty :
                string.Format(" lower({0}.leagueEn) like '%{1}%' ", matches, userProfile.LeagueKeyword.ToLower());
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

            return bettypeIds.Any() ? string.Format(" {0}.betTypeId in [{1}] ", matchOddsList, string.Join(",", bettypeIds))
                : string.Empty;
        }

        private string BuildFilterByTimeStamp(string timeStamp, string matchOddsList)
        {
            var changeTime = !string.IsNullOrEmpty(timeStamp);
            var _timeStamp = changeTime ? int.Parse(timeStamp) : 0;

            return changeTime ? string.Format(" {0}.timeStamp > {1} ", _timeStamp) : string.Empty;
        }
    }
}
