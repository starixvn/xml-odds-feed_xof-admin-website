namespace Fanex.OddsFeed.WindowsServiceLibrary.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common;
    using Common.Configurations;
    using Common.DTO;
    using Common.Helpers;
    using Common.Schemas;
    using Couchbase;
    using Couchbase.Configuration.Client;
    using Couchbase.Core;
    using Mapping;

    public class WindowsOddsFeedService : IWindowsOddsFeedService
    {
        private static readonly string _cachePrefix = AppSettings.CachePrefix;
        private static readonly string _oddsFeedBucket = AppSettings.BucketOddsFeed;
        private static readonly List<Uri> _couchBaseServer = new List<Uri> { new Uri(AppSettings.CouchbaseServer) };
        private static readonly string _currentVersionKey = CacheKeys.CurrentKey(AppSettings.CachePrefix);
        private static int _totalVersion = AppSettings.TotalVersion;

        public void Reset()
        {
            using (var cluster = new Cluster(new ClientConfiguration { Servers = _couchBaseServer }))
            {
                Store(cluster, AppSettings.BucketOddsFeed, AppSettings.BetTypeSchemaKey, new List<BetTypeSchema>());
                Store(cluster, AppSettings.BucketOddsFeed, CacheKeys.CurrentKey(_cachePrefix), -1);
                Store(cluster, AppSettings.BucketOddsFeed, CacheKeys.FullChainKey(_cachePrefix), false);
            }
        }

        public bool Update()
        {
            var result = false;

            var betTypeSchemas = GetBetTypeSchemas();
            var liveMatches = GetLiveMatches(betTypeSchemas);
            var deadBallMatches = GetDeadballMatches(betTypeSchemas);
            var outrights = GetOutrights();

            using (var cluster = new Cluster(new ClientConfiguration { Servers = _couchBaseServer }))
            {
                var currentVersion = GetCurrentVersion(cluster) + 1;
                if (currentVersion == _totalVersion)
                {
                    currentVersion = 0;
                }
                var versionKey = currentVersion.ToString();

                result = Store(cluster, AppSettings.BucketOddsFeed, CacheKeys.CurrentKey(_cachePrefix), currentVersion);

                result &= Store(cluster, AppSettings.BucketLiveMatches, versionKey, liveMatches);
                result &= Store(cluster, AppSettings.BucketDeadMatches, versionKey, deadBallMatches);
                result &= Store(cluster, AppSettings.BucketOutrights, versionKey, outrights);

                if (result && currentVersion == _totalVersion - 1)
                {
                    result &= Store(cluster, AppSettings.BucketOddsFeed, CacheKeys.FullChainKey(_cachePrefix), true);
                }
            }

            return result;
        }

        private static IList<Match> GetLiveMatches(IList<BetTypeSchema> betTypeSchemas)
        {
            using (var liveOdds = DbAccess.GetLiveOddsData())
            {
                var matchMapping = new MatchMapping();
                return matchMapping.GetMatches(liveOdds.Tables[0], betTypeSchemas, GetOddsFlexGroups(liveOdds.Tables[1]), true);
            }
        }

        private static IList<Match> GetDeadballMatches(IList<BetTypeSchema> betTypeSchemas)
        {
            using (var deadballOdds = DbAccess.GetDeadballOddsData())
            {
                var matchMapping = new MatchMapping();
                return matchMapping.GetMatches(deadballOdds.Tables[0], betTypeSchemas, GetOddsFlexGroups(deadballOdds.Tables[1]), false);
            }
        }

        private static IList<Outright> GetOutrights()
        {
            using (var outrightOdds = DbAccess.GetOutrightOddsData())
            {
                var outrightMapping = new OutrightMapping();
                return outrightMapping.GetOutrights(outrightOdds);
            }
        }

        private static IEnumerable<OddsFlexGroup> GetOddsFlexGroups(DataTable table)
        {
            var oddsFlexGroups = new List<OddsFlexGroup>();

            foreach (DataRow row in table.Rows)
            {
                var oddsId = Convert.ToInt32(row["oddsid"]);
                var oddsFlex = new OddsFlex
                {
                    BetChoice = row["betChoice"].ToString(),
                    BetType = Convert.ToInt32(row["bettype"]),
                    OddId = oddsId,
                    Odds = Convert.ToDecimal(row["odds"]),
                    OddsInfo = row["OddsInfo"].ToString()
                };
                var oddsFlexGroup = oddsFlexGroups.FirstOrDefault(odds => odds.OddsId == oddsId);

                if (oddsFlexGroup == null)
                {
                    oddsFlexGroups.Add(new OddsFlexGroup
                    {
                        OddsId = oddsId,
                        OddsFlexs = new List<OddsFlex> { oddsFlex }
                    });
                }
                else
                {
                    oddsFlexGroup.OddsFlexs.Add(oddsFlex);
                }
            }

            return oddsFlexGroups;
        }

        private static IList<BetTypeSchema> GetBetTypeSchemas()
        {
            using (var cluster = new Cluster(new ClientConfiguration { Servers = _couchBaseServer }))
            {
                using (var bucket = cluster.OpenBucket(AppSettings.BucketOddsFeed))
                {
                    var document = bucket.Get<IList<BetTypeSchema>>(AppSettings.BetTypeSchemaKey);

                    if (document == null || !document.Value.Any())
                    {
                        var newValue = JsonHelper.GetBetTypeSchemas(AppSettings.BetTypeSchemaPath);

                        if (newValue != null)
                        {
                            bucket.Upsert(AppSettings.BetTypeSchemaKey, newValue);

                            return newValue;
                        }
                        return new List<BetTypeSchema>();
                    }

                    return document.Value;
                }
            }
        }

        private static bool Store<T>(ICluster cluster, string bucketName, string key, T value)
        {
            using (var bucket = cluster.OpenBucket(bucketName))
            {
                var upsert = bucket.Upsert(key, value);

                return upsert.Success;
            }
        }

        private static int GetCurrentVersion(ICluster cluster)
        {
            var currentVersion = 0;

            using (var bucket = cluster.OpenBucket(_oddsFeedBucket))
            {
                var get = bucket.GetDocument<int>(_currentVersionKey);
                currentVersion = get.Document.Content;
            }

            return currentVersion;
        }
    }
}