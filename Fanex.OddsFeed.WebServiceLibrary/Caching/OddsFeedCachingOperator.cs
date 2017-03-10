namespace Fanex.OddsFeed.WebServiceLibrary.Caching
{
    using System;
    using System.Collections.Generic;
    using Couchbase;
    using Couchbase.Configuration.Client;
    using Couchbase.Core;
    using Fanex.OddsFeed.Common.Configurations;
    using Fanex.OddsFeed.Common.DTO;
    using Models;

    public class OddsFeedCachingOperator : IOddsFeedCachingOperator
    {
        private readonly ICluster _cluster;
        private readonly IOutrightCaching _outrightCaching;
        private readonly IMatchCaching _livematchCaching;
        private readonly IMatchCaching _deadmatchCaching;

        public OddsFeedCachingOperator()
        {
            _cluster = new Cluster(new ClientConfiguration
            {
                Servers = new List<Uri> { new Uri(AppSettings.CouchbaseServer) }
            });

            _outrightCaching = new OutrightCaching();
            _livematchCaching = new MatchCaching(true);
            _deadmatchCaching = new MatchCaching(false);
        }

        public SafeList<Match> GetMatches(OddsFeedsModel model, bool isLive)
        {
            return isLive ? _livematchCaching.GetMatches(_cluster, model.UserProfile, model.UserQuery) :
                _deadmatchCaching.GetMatches(_cluster, model.UserProfile, model.UserQuery);
        }

        public SafeList<Outright> GetOutrights(OddsFeedsModel model)
        {
            return _outrightCaching.GetOutrights(_cluster, model.UserProfile, model.UserQuery);
        }
    }
}
