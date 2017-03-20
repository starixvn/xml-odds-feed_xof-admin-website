namespace Fanex.OddsFeed.WebServiceLibrary.Caching
{
    using System;
    using System.Collections.Generic;
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
            var outrights = isGetOutright ? GetOdds<Outright>(AppSettings.BucketOutrights, cacheVersion.CurrentVersionOutright) : new List<Outright>();
            var liveMatches = isGetLiveMatch ? GetOdds<Match>(AppSettings.BucketLiveMatches, cacheVersion.CurrentVersionLiveMatch) : new List<Match>();
            var deadballMatches = isGetDeadballMatch ? GetOdds<Match>(AppSettings.BucketDeadMatches, cacheVersion.CurrentVersionDeadballMatch) : new List<Match>();

            return new OddsData
            {
                Outrights = outrights,
                LiveMatches = liveMatches,
                DeadballMatches = deadballMatches
            };
        }

        private IList<T> GetOdds<T>(string bucketName, int cacheVersion)
        {
            using (var bucket = _cluster.OpenBucket(bucketName))
            {
                var result = bucket.Get<IList<T>>(cacheVersion.ToString());

                return result.Success ? result.Value : new List<T>();
            }
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
    }
}
