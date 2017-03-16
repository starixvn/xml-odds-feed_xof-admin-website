using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Couchbase;
using Couchbase.Configuration.Client;

namespace CouchbaseApp
{
    public class LeagueDocumentTest
    {
        private readonly string _serverUrl = "http://10.35.35.253:8091/pools";
        private readonly Cluster _cluster;
        private readonly FakeDataV1 _fakeData;
        private readonly string _liveMatchBucket = "livematches";
        private readonly string _deadMatchBucket = "deadmatches";
        private readonly string _outrightBucket = "outrights";
        private readonly string _oddsfeedBucket = "oddsfeed";

        public LeagueDocumentTest()
        {
            var config = new ClientConfiguration
            {
                Servers = new List<Uri> { new Uri(_serverUrl) }
            };

            _cluster = new Cluster(config);
            _fakeData = new FakeDataV1();
        }
    }
}
