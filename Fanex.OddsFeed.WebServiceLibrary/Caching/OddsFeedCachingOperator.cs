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
        private readonly IMatchCaching _matchCaching;

        public OddsFeedCachingOperator()
        {
            _cluster = new Cluster(new ClientConfiguration
            {
                Servers = new List<Uri> { new Uri(AppSettings.CouchbaseServer) }
            });

            _outrightCaching = new OutrightCaching();
            _matchCaching = new MatchCaching();
        }

        public SafeList<Match> GetMatches(OddsFeedsModel model, bool isLive)
        {
            return _matchCaching.GetMatches(model.UserProfile, model.UserQuery, isLive);
        }

        public SafeList<Outright> GetOutrights(OddsFeedsModel model)
        {
            return _outrightCaching.GetOutrights(_cluster, model.UserProfile, model.UserQuery);
        }
    }
}
