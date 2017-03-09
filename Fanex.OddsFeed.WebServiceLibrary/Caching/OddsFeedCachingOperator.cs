using System;
using System.Collections.Generic;
using Couchbase;
using Couchbase.Configuration.Client;
using Couchbase.Core;
using Fanex.OddsFeed.Common.Configurations;
using Fanex.OddsFeed.Common.DTO;

namespace Fanex.OddsFeed.WebServiceLibrary.Caching
{
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

        public OddsData GetOddsData(UserProfile userProfile, UserQuery userQuery)
        {
            return new OddsData
            {
                Outrights = GetOutrights(userProfile, userQuery),
                LiveMatches = GetMatches(userProfile, userQuery, true),
                DeadballMatches = GetMatches(userProfile, userQuery, false)
            };
        }

        private SafeList<Match> GetMatches(UserProfile userProfile, UserQuery userQuery, bool isLive)
        {
            return _matchCaching.GetMatches(userProfile, userQuery, isLive);
        }

        private SafeList<Outright> GetOutrights(UserProfile userProfile, UserQuery userQuery)
        {
            return _outrightCaching.GetOutrights(_cluster, userProfile, userQuery);
        }
    }
}
