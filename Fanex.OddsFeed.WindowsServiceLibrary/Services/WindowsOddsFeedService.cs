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
    using Logging;
    using Mapping;

    public class WindowsOddsFeedService : IWindowsOddsFeedService
    {
        private readonly string _cachePrefix;
        private readonly IMatchMapping _matchMapping;
        private readonly IOutrightMapping _outrightMapping;
        private readonly Cluster _cluster;

        public WindowsOddsFeedService()
        {
            _cachePrefix = AppSettings.CachePrefix;
            _matchMapping = new MatchMapping();
            _outrightMapping = new OutrightMapping();

            _cluster = new Cluster(new ClientConfiguration
            {
                Servers = new List<Uri> { new Uri(AppSettings.CouchbaseServer) }
            });
        }

        public void Reset()
        {
            StoreValue(AppSettings.BucketOddsFeed, AppSettings.BetTypeSchemaKey, new List<BetTypeSchema>());
            StoreValue(AppSettings.BucketOddsFeed, CacheKeys.CurrentKey(_cachePrefix), 0);
            StoreValue(AppSettings.BucketOddsFeed, CacheKeys.FullChainKey(_cachePrefix), false);
        }

        public bool StoreFullChain()
        {
            return StoreValue(AppSettings.BucketOddsFeed, CacheKeys.FullChainKey(_cachePrefix), true);
        }

        
        public bool Update(int currentVersion)
        {
            var result = false;

            try
            {
                var betTypeSchemas = GetBetTypeSchemas();
                var liveMatches = GetLiveMatches(betTypeSchemas);
                var deadBallMatches = GetDeadballMatches(betTypeSchemas);
                var outrights = GetOutrights();

                result = StoreValue(AppSettings.BucketOddsFeed, CacheKeys.CurrentKey(_cachePrefix), currentVersion);

                result = result && StoreOdds(
                    AppSettings.BucketLiveMatches, 
                    new Document<MatchDocument>
                    {
                        Id = currentVersion.ToString(),
                        Content = new MatchDocument { Version = currentVersion, Matches = liveMatches }
                    });

                result = result && StoreOdds(
                    AppSettings.BucketDeadMatches,
                    new Document<MatchDocument>
                    {
                        Id = currentVersion.ToString(),
                        Content = new MatchDocument { Version = currentVersion, Matches = deadBallMatches }
                    });

                result = result && StoreOdds(
                    AppSettings.BucketOutrights,
                    new Document<OutrightDocument>
                    {
                        Id = currentVersion.ToString(),
                        Content = new OutrightDocument { Version = currentVersion, Outrights = outrights }
                    });

                return result;
            }
            catch (Exception exception)
            {
                var innerExceptionMessage = exception.InnerException == null ? "No inner" : exception.InnerException.Message;
                var message = $"Main error: {exception.Message}. Inner error:{innerExceptionMessage} - stackstrace:{exception.StackTrace}";

                LogService.Error(message, exception);
            }

            return true;
        }

        private IList<Match> GetLiveMatches(IList<BetTypeSchema> betTypeSchemas) 
        {
            using (var liveOdds = DbAccess.GetLiveOddsData())
            {
                return _matchMapping.GetMatches(liveOdds.Tables[0], betTypeSchemas, GetOddsFlexGroups(liveOdds.Tables[1]));
            }
        }

        private IList<Match> GetDeadballMatches(IList<BetTypeSchema> betTypeSchemas)
        {
            using (var deadballOdds = DbAccess.GetDeadballOddsData())
            {
                return _matchMapping.GetMatches(deadballOdds.Tables[0], betTypeSchemas, GetOddsFlexGroups(deadballOdds.Tables[1]));
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

        private IList<BetTypeSchema> GetBetTypeSchemas()
        {
            using (var bucket = _cluster.OpenBucket(AppSettings.BucketOddsFeed))
            {
                var document = bucket.GetDocument<dynamic>(AppSettings.BetTypeSchemaKey);
                var schemas = document.Content as IList<BetTypeSchema>;

                if (schemas == null || !schemas.Any())
                {
                    schemas = JsonHelper.GetBetTypeSchemas(AppSettings.BetTypeSchemaPath);

                    if (schemas != null)
                    {
                        bucket.Upsert(new Document<dynamic> { Id = AppSettings.BetTypeSchemaKey, Content = schemas });

                        return schemas;
                    }
                }

                return schemas;
            }
        }

        private bool StoreOdds<T>(string bucketName, Document<T> document)
        {
            using (var bucket = _cluster.OpenBucket(bucketName))
            {
                var upsert = bucket.Upsert(document);

                return upsert.Success;
            }
        }

        private bool StoreValue(string bucketName, string key, object value)
        {
            using (var bucket = _cluster.OpenBucket(bucketName))
            {
                var upsert = bucket.Upsert(new Document<dynamic> { Id = key, Content = value });

                return upsert.Success;
            }
        }
    }
}