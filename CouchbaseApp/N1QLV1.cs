using System;
using System.Collections.Generic;
using Couchbase;
using Couchbase.Configuration.Client;

namespace CouchbaseApp
{
    public class N1QLV1
    {
        private readonly string _serverUrl = "http://10.35.35.254:8091/pools";
        private readonly Cluster _cluster;
        private readonly FakeDataV1 _fakeData;
        private readonly string _liveMatchBucket = "livematches";
        private readonly string _deadMatchBucket = "deadmatches";
        private readonly string _outrightBucket = "outrights";
        private readonly string _oddsfeedBucket = "oddsfeed";

        public N1QLV1()
        {
            var config = new ClientConfiguration
            {
                Servers = new List<Uri> { new Uri(_serverUrl) }
            };

            _cluster = new Cluster(config);
            _fakeData = new FakeDataV1();
        }

        public void StoreData()
        {
            //DropIndexDeadMatch();
            //DropIndexLiveMatch();
            //DropIndexOutright();
            //DropIndexOddsFeed();

            StoreDataDeadMatch();
            StoreDataLiveMatch();
            //StoreDataOutright();
            //StoreDataOddsFeed();

            //CreateIndexDeadMatch();
            //CreateIndexLiveMatch();
            //CreateIndexOutright();
            //CreateIndexOddsFeed();
        }

        private void StoreDataDeadMatch()
        {
            using (var bucket = _cluster.OpenBucket(_deadMatchBucket))
            {
                for (var i = 0; i < 62; i++)
                {
                    var matches = _fakeData.FakeDeadMatches(i);
                    foreach (var match in matches)
                    {
                        bucket.Insert(match);
                    }
                }
            }
        }

        private void StoreDataLiveMatch()
        {
            using (var bucket = _cluster.OpenBucket(_liveMatchBucket))
            {
                for (var i = 0; i < 62; i++)
                {
                    var matches = _fakeData.FakeLiveMatches(i);
                    foreach (var match in matches)
                    {
                        bucket.Insert(match);
                    }
                }
            }
        }

        private void StoreDataOutright()
        {
            using (var bucket = _cluster.OpenBucket(_outrightBucket))
            {
                for (var i = 0; i < 62; i++)
                {
                    var outrights = _fakeData.FakeOutrights(i);
                    bucket.InsertAsync(outrights);
                }
            }
        }

        private void StoreDataOddsFeed()
        {
            using (var bucket = _cluster.OpenBucket(_oddsfeedBucket))
            {
                var schemaDoc = new Document<dynamic>
                {
                    Id = "BetTypeSchemaKey",
                    Content = new int[] { }
                };
                bucket.InsertAsync(schemaDoc);

                var currentVersionDoc = new Document<dynamic>
                {
                    Id = "apollocurrent",
                    Content = 0
                };
                bucket.InsertAsync(currentVersionDoc);

                var fullChainDoc = new Document<dynamic>
                {
                    Id = "apollofullchain",
                    Content = true
                };
                bucket.InsertAsync(fullChainDoc);
            }
        }

        private void CreateIndexLiveMatch()
        {
            var bucketName = _liveMatchBucket;
            var queries = new[] {
                "CREATE PRIMARY INDEX `#primary` ON " + bucketName,
                "CREATE INDEX `ix_sportid` ON " +  bucketName + "(sportId)",
                "CREATE INDEX `ix_version` ON " +  bucketName + "(version)",
                "CREATE INDEX `ix_leagueId` ON " +  bucketName + "(leagueId)",
                "CREATE INDEX `ix_leagueEn` ON " +  bucketName + "(leagueEn)",
                "CREATE INDEX `ix_countryCode` ON " +  bucketName + " (countryCode)"

            };

            using (var bucket = _cluster.OpenBucket(bucketName))
            {
                foreach (var query in queries)
                {
                    bucket.Query<dynamic>(query);
                }
            }
        }

        private void CreateIndexDeadMatch()
        {
            var bucketName = _deadMatchBucket;
            var queries = new[] {
                "CREATE PRIMARY INDEX `#primary` ON " + bucketName,
                "CREATE INDEX `ix_sportid` ON " +  bucketName + "(sportId)",
                "CREATE INDEX `ix_version` ON " +  bucketName + "(version)",
                "CREATE INDEX `ix_leagueId` ON " +  bucketName + "(leagueId)",
                "CREATE INDEX `ix_leagueEn` ON " +  bucketName + "(leagueEn)",
                "CREATE INDEX `ix_countryCode` ON " +  bucketName + " (countryCode)"

            };

            using (var bucket = _cluster.OpenBucket(bucketName))
            {
                foreach (var query in queries)
                {
                    bucket.Query<dynamic>(query);
                }
            }
        }

        private void CreateIndexOutright()
        {
            var bucketName = _outrightBucket;
            var queries = new[] {
                "CREATE PRIMARY INDEX `#primary` ON " + bucketName,
                "CREATE INDEX `ix_sportid` ON " +  bucketName + "(sportId)",
                "CREATE INDEX `ix_version` ON " +  bucketName + "(version)",
                "CREATE INDEX `ix_eventId` ON " +  bucketName + "(eventId)",
                "CREATE INDEX `ix_eventEn` ON " +  bucketName + "(eventEn)",
                "CREATE INDEX `ix_countryCode` ON " +  bucketName + " (countryCode)"

            };

            using (var bucket = _cluster.OpenBucket(bucketName))
            {
                foreach (var query in queries)
                {
                    bucket.Query<dynamic>(query);
                }
            }
        }

        private void CreateIndexOddsFeed()
        {
            var bucketName = _oddsfeedBucket;
            var queries = new[] {
                "CREATE PRIMARY INDEX `#primary` ON " + bucketName
            };

            using (var bucket = _cluster.OpenBucket(bucketName))
            {
                foreach (var query in queries)
                {
                    bucket.Query<dynamic>(query);
                }
            }
        }

        private void DropIndexLiveMatch()
        {
            var bucketName = _liveMatchBucket;
            var queries = new[] {
                "DROP INDEX " +  bucketName + ".`#primary`",
                "DROP INDEX " +  bucketName + ".`ix_sportid`",
                "DROP INDEX " +  bucketName + ".`ix_version`",
                "DROP INDEX " +  bucketName + ".`ix_leagueId`",
                "DROP INDEX " +  bucketName + ".`ix_leagueEn`",
                "DROP INDEX " +  bucketName + ".`ix_countryCode`"

            };

            using (var bucket = _cluster.OpenBucket(bucketName))
            {
                foreach (var query in queries)
                {
                    bucket.Query<dynamic>(query);
                }
            }
        }

        private void DropIndexDeadMatch()
        {
            var bucketName = _deadMatchBucket;
            var queries = new[] {
                "DROP INDEX " +  bucketName + ".`#primary`",
                "DROP INDEX " +  bucketName + ".`ix_sportid`",
                "DROP INDEX " +  bucketName + ".`ix_version`",
                "DROP INDEX " +  bucketName + ".`ix_leagueId`",
                "DROP INDEX " +  bucketName + ".`ix_leagueEn`",
                "DROP INDEX " +  bucketName + ".`ix_countryCode`"

            };

            using (var bucket = _cluster.OpenBucket(bucketName))
            {
                foreach (var query in queries)
                {
                    bucket.Query<dynamic>(query);
                }
            }
        }

        private void DropIndexOutright()
        {
            var bucketName = _outrightBucket;
            var queries = new[] {
                "DROP INDEX " +  bucketName + ".`#primary`",
                "DROP INDEX " +  bucketName + ".`ix_sportid`",
                "DROP INDEX " +  bucketName + ".`ix_version`",
                "DROP INDEX " +  bucketName + ".`ix_eventId`",
                "DROP INDEX " +  bucketName + ".`ix_eventEn`",
                "DROP INDEX " +  bucketName + ".`ix_countryCode`"

            };

            using (var bucket = _cluster.OpenBucket(bucketName))
            {
                foreach (var query in queries)
                {
                    bucket.Query<dynamic>(query);
                }
            }
        }

        private void DropIndexOddsFeed()
        {
            var bucketName = _oddsfeedBucket;
            var queries = new[] {
                "DROP INDEX " +  bucketName + ".`#primary`"
            };

            using (var bucket = _cluster.OpenBucket(bucketName))
            {
                foreach (var query in queries)
                {
                    bucket.Query<dynamic>(query);
                }
            }
        }
    }
}
