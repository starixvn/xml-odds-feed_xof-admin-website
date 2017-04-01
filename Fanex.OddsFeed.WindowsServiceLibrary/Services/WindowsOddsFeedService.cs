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
    using Mapping;
    using Couchbase.Core;
    using System.Runtime.Caching;

    public class WindowsOddsFeedService : IWindowsOddsFeedService
    {
        private readonly string _cachePrefix;
        private readonly IMatchMapping _matchMapping;
        private readonly IOutrightMapping _outrightMapping;
        private readonly IList<Uri> _couchBaseServerList;
        private ClientConfiguration _couchBaseClientConfig;

        public WindowsOddsFeedService()
        {
            _cachePrefix = AppSettings.CachePrefix;
            _matchMapping = new MatchMapping();
            _outrightMapping = new OutrightMapping();
            _couchBaseServerList = new List<Uri> { new Uri(AppSettings.CouchbaseServer) };
            _couchBaseClientConfig = new ClientConfiguration
            {
                Servers = new List<Uri> { new Uri(AppSettings.CouchbaseServer) }

            };
        }

        public void Reset()
        {
            using (var cluster = new Cluster(_couchBaseClientConfig))
            {
                Store(cluster, AppSettings.BucketOddsFeed, AppSettings.BetTypeSchemaKey, new List<BetTypeSchema>());
                Store(cluster, AppSettings.BucketOddsFeed, CacheKeys.CurrentKey(_cachePrefix), 0);
                Store(cluster, AppSettings.BucketOddsFeed, CacheKeys.FullChainKey(_cachePrefix), false);
            }
        }

        public bool StoreFullChain()
        {
            using (var cluster = new Cluster(_couchBaseClientConfig))
            {
                return Store(cluster, AppSettings.BucketOddsFeed, CacheKeys.FullChainKey(_cachePrefix), true);
            }
        }


        public bool Update(int currentVersion)
        {
            var result = false;

            using (var cluster = new Cluster(_couchBaseClientConfig))
            {

                var betTypeSchemas = GetBetTypeSchemas(cluster);
                var liveMatches = GetLiveMatches(betTypeSchemas);
                var deadBallMatches = GetDeadballMatches(betTypeSchemas);
                var outrights = GetOutrights();


                result = Store(cluster, AppSettings.BucketOddsFeed, CacheKeys.CurrentKey(_cachePrefix), currentVersion);

                result = result && Store(cluster, AppSettings.BucketLiveMatches, currentVersion.ToString(), liveMatches);
                result = result && Store(cluster, AppSettings.BucketDeadMatches, currentVersion.ToString(), deadBallMatches);
                result = result && Store(cluster, AppSettings.BucketOutrights, currentVersion.ToString(), outrights);

                return result;
            }
        }

        private IList<Match> GetLiveMatches(IList<BetTypeSchema> betTypeSchemas)
        {
            using (var liveOdds = DbAccess.GetLiveOddsData())
            {
                return _matchMapping.GetMatches(liveOdds.Tables[0], betTypeSchemas, GetOddsFlexGroups(liveOdds.Tables[1]), true);
            }
        }

        private IList<Match> GetDeadballMatches(IList<BetTypeSchema> betTypeSchemas)
        {
            using (var deadballOdds = DbAccess.GetDeadballOddsData())
            {
                return _matchMapping.GetMatches(deadballOdds.Tables[0], betTypeSchemas, GetOddsFlexGroups(deadballOdds.Tables[1]), false);
            }
        }

        private IList<Outright> GetOutrights()
        {
            using (var outrightOdds = DbAccess.GetOutrightOddsData())
            {
                return _outrightMapping.GetOutrights(outrightOdds);
            }
        }

        private static IEnumerable<OddsFlex> GetOddFlexs(DataTable table)
        {
            return table.AsEnumerable()
                .Select(
                row => new OddsFlex
                {
                    BetChoice = row["betChoice"].ToString(),
                    BetType = Convert.ToInt32(row["bettype"]),
                    OddId = Convert.ToInt32(row["oddsid"]),
                    Odds = Convert.ToDecimal(row["odds"]),
                    OddsInfo = row["OddsInfo"].ToString()
                }).ToList();
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

        private IList<BetTypeSchema> GetBetTypeSchemas(ICluster cluster)
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

        private bool Store<T>(ICluster cluster, string bucketName, string key, T value)
        {
            using (var bucket = cluster.OpenBucket(bucketName))
            {
                var upsert = bucket.Upsert(key, value);

                return upsert.Success;
            }
        }
    }
}