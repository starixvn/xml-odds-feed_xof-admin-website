namespace Fanex.OddsFeed.WebServiceLibrary.Caching
{
    using System;
    using System.Collections.Generic;
    using Common.Schemas;
    using Couchbase;
    using Couchbase.Configuration.Client;
    using Couchbase.Core;
    using Fanex.OddsFeed.Common.Configurations;
    using Fanex.OddsFeed.Common.DTO;
    using Logging;
    using Models;

    public class OddsFeedCachingOperator : IOddsFeedCachingOperator, IDisposable
    {
        private readonly ICluster _cluster;
        private readonly IVersionCaching _versionCaching;

        public OddsFeedCachingOperator()
        {
            _cluster = new Cluster(new ClientConfiguration
            {
                Servers = new List<Uri> { new Uri(AppSettings.CouchbaseServer) }
            });

            _versionCaching = new VersionCaching();
        }

        public OddsData GetOddsData(OddsFeedsModel model, bool isGetOutright, bool isGetLiveMatch, bool isGetDeadballMatch)
        {
            var cacheVersion = _versionCaching.GetRightVersion(_cluster, model);

            var outrights = isGetOutright 
                ? Get<Outright>(AppSettings.BucketOutrights, cacheVersion.CurrentVersionOutright.ToString()) : 
                new List<Outright>();

            var liveMatches = isGetLiveMatch 
                ? Get<Match>(AppSettings.BucketLiveMatches, cacheVersion.CurrentVersionLiveMatch.ToString()) 
                : new List<Match>();

            var deadballMatches = isGetDeadballMatch 
                ? Get<Match>(AppSettings.BucketDeadMatches, cacheVersion.CurrentVersionDeadballMatch.ToString()) 
                : new List<Match>();

            return new OddsData
            {
                Outrights = outrights,
                LiveMatches = liveMatches,
                DeadballMatches = deadballMatches
            };
        }

        public IList<BetTypeSchema> GetBetTypes()
        {
            return Get<BetTypeSchema>(AppSettings.BucketOddsFeed, AppSettings.BetTypeSchemaKey);
        }

        public void Dispose()
        {
            try
            {
                _cluster.Dispose();
            }
            catch (Exception exception)
            {
                LogService.Error(exception.Message, exception);
            }

            GC.SuppressFinalize(this);
        }

        private IList<T> Get<T>(string bucketName, string key)
        {
            using (var bucket = _cluster.OpenBucket(bucketName))
            {
                var result = bucket.Get<IList<T>>(key);

                return result.Success ? result.Value : new List<T>();
            }
        }
    }
}
