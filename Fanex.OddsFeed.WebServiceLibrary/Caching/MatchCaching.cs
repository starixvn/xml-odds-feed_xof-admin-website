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

            data = FilterByBetType(model.UserProfile, model.UserQuery, data);
            data = FilterByTimeStamp(model.TimeStamp, data);
            return new SafeList<Match>(data);
        }

        private string BuildQuery(OddsFeedsModel model, int cacheVersion)
        {
            var matches = "matches";
            var queryTemplate = @"select matches.* 
                                  from {0} as matches
                                  {1}";
            var conditions = new[] {
                BuildFilterByVersion(matches, cacheVersion),
                BuildFilterBySport(model.UserProfile, model.UserQuery, matches),
                BuildFilterByUserProfileLeague(model.UserProfile, matches),
                BuildFilterByUserQueryLeague(model.UserQuery, matches),
                BuildFilterByCountry(model.UserProfile, model.UserQuery, matches),
                BuildFilterByKeyword(model.UserProfile, matches)
            }.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            var whereClause = conditions.Any() ? $" where {string.Join(" and ", conditions)} " : string.Empty;

            return string.Format(queryTemplate, _matchBucket, whereClause);
        }

        private string BuildFilterByVersion(string matches, int cacheVersion)
        {
            return $" {matches}.version = {cacheVersion} ";
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

            return leagues.Any() ? $" {matches}.leagueId in [{string.Join(",", leagues)}] " : string.Empty;
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

        private List<Match> FilterByBetType(UserProfile userProfile, UserQuery userQuery, List<Match> data)
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

            if (bettypeIds.Any())
            {
                foreach (var match in data)
                {
                    match.MatchOddsList = match.MatchOddsList.Where(x => bettypeIds.Contains(x.BetTypeId)).ToList();
                }
            }

            return data;
        }

        private List<Match> FilterByTimeStamp(int timeStamp, List<Match> data)
        {
            //var changeTime = !string.IsNullOrEmpty(timeStamp);
            //var _timeStamp = changeTime ? int.Parse(timeStamp) : 0;

            //if (changeTime)
            {
                foreach (var match in data)
                {
                    match.MatchOddsList = match.MatchOddsList.Where(x => x.TimeStamp > timeStamp).ToList();
                }
            }

            return data;
        }
    }
}
