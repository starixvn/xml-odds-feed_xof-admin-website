using System.Collections.Generic;
using System.Linq;
using Couchbase.Core;
using Fanex.OddsFeed.Common.Configurations;
using Fanex.OddsFeed.Common.DTO;
using Fanex.OddsFeed.WebServiceLibrary.Models;

namespace Fanex.OddsFeed.WebServiceLibrary.Caching
{
    public class OutrightCaching : IOutrightCaching
    {
        private readonly string _outrightBucket;

        public OutrightCaching()
        {
            _outrightBucket = AppSettings.BucketOutrights;
        }

        public SafeList<Outright> GetOutrights(ICluster cluster, OddsFeedsModel model, int cacheVersion)
        {
            var data = new List<Outright> { };
            using (var bucket = cluster.OpenBucket(_outrightBucket))
            {
                var query = BuildQuery(model.UserProfile, model.UserQuery, cacheVersion);
                data = bucket.Query<Outright>(query).ToList();
            }

            return new SafeList<Outright>(data);
        }

        private string BuildQuery(UserProfile userProfile, UserQuery userQuery, int cacheVersion)
        {
            var outrightList = "outrightList";
            var query = $"select outrightList.* from {_outrightBucket} unnest {_outrightBucket}.outrights as {outrightList}";
            var conditions = new[] {
                BuildFilterByVersion(_outrightBucket, cacheVersion),
                BuildFilterBySport(userProfile, userQuery, outrightList),
                BuildFilterByLeague(userQuery, outrightList),
                BuildFilterByCountry(userProfile, userQuery, outrightList),
                BuildFilterByProfile(userProfile, outrightList),
                BuildFilterByKeyword(userProfile, outrightList)
            }.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            query += conditions.Any() ? $" where {string.Join(" and ", conditions)} " : string.Empty;

            return query;
        }

        private string BuildFilterByVersion(string outrightBucket, int cacheVersion)
        {
            return $" {outrightBucket}.version = {cacheVersion} ";
        }

        private string BuildFilterBySport(UserProfile userProfile, UserQuery userQuery, string outrightList)
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

            return sportIds.Any() ? $" {outrightList}.sportId in [{string.Join(",", sportIds)}] " : string.Empty;
        }

        private string BuildFilterByLeague(UserQuery userQuery, string outrightList)
        {
            var leagues = new List<string>();
            if (userQuery.Leagues != null && userQuery.Leagues.Any())
            {
                leagues.AddRange(userQuery.Leagues);
            }

            leagues = leagues.Where(l => l != null).Select(l => { return "'" + l.ToLower() + "'"; }).ToList();

            return leagues.Any() ? $" lower({outrightList}.eventEn) in [{string.Join(",", leagues)}] " : string.Empty;
        }

        private string BuildFilterByCountry(UserProfile userProfile, UserQuery userQuery, string outrightList)
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

            return countries.Any() ? $" lower({outrightList}.countryCode) in [{string.Join(",", countries)}] " : string.Empty;
        }

        private string BuildFilterByProfile(UserProfile userProfile, string outrightList)
        {
            var eventIds = string.Empty;
            if (userProfile.Outrights != null && userProfile.Outrights.Any())
            {
                eventIds += string.Join(",", userProfile.Outrights);
            }

            return string.IsNullOrEmpty(eventIds) ? string.Empty : $" {outrightList}.eventId in [{eventIds}] ";
        }

        private string BuildFilterByKeyword(UserProfile userProfile, string outrightList)
        {
            return string.IsNullOrEmpty(userProfile.OutrightsKeyword) 
                ? string.Empty 
                : $" lower({outrightList}.eventEn) like '%{userProfile.OutrightsKeyword.ToLower()}%' ";
        }
    }
}
