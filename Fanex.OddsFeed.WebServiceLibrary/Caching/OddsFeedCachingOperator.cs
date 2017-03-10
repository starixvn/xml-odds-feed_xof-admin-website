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
        private readonly IOutrightCaching _outrightCaching;
        private readonly IMatchCaching _livematchCaching;
        private readonly IMatchCaching _deadmatchCaching;

        public OddsFeedCachingOperator()
        {
            _cluster = new Cluster(new ClientConfiguration
            {
                Servers = new List<Uri> { new Uri(AppSettings.CouchbaseServer) }
            });

            _versionCaching = new VersionCaching();
            _outrightCaching = new OutrightCaching();
            _livematchCaching = new MatchCaching(true);
            _deadmatchCaching = new MatchCaching(false);
        }

        public OddsData GetOddsData(OddsFeedsModel model, bool isGetOutright, bool isGetLiveMatch, bool isGetDeadballMatch)
        {
            var cacheVersion = _versionCaching.GetRightVersion(_cluster, model);
            var outrights = isGetOutright ? GetOutrights(model, cacheVersion) : new SafeList<Outright>();
            var liveMatches = isGetLiveMatch ? GetMatches(model, cacheVersion, true) : new SafeList<Match>();
            var deadballMatches = isGetDeadballMatch ? GetMatches(model, cacheVersion, false) : new SafeList<Match>();

            return new OddsData
            {
                Outrights = outrights,
                LiveMatches = liveMatches,
                DeadballMatches = deadballMatches
            };
        }

        private SafeList<Match> GetMatches(OddsFeedsModel model, CacheVersion cacheVersion, bool isLive)
        {
            return isLive ? _livematchCaching.GetMatches(_cluster, model, cacheVersion) :
                _deadmatchCaching.GetMatches(_cluster, model, cacheVersion);
        }

        private SafeList<Outright> GetOutrights(OddsFeedsModel model, CacheVersion cacheVersion)
        {
            return _outrightCaching.GetOutrights(_cluster, model, cacheVersion);
        }

        public SafeList<Match> GetEventMatches(bool isLive)
        {
            return new SafeList<Match>();
        }

        public SafeList<Outright> GetEventOutrights()
        {
            return new SafeList<Outright>();
        }

        public void Dispose()
        {
            try
            {
                _cluster.Dispose();
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
            }
        }
    }
}
