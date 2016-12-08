namespace Fanex.OddsFeed.WebServiceLibrary.Caching
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Configurations;
    using Common.DTO;
    using Common.Schemas;
    using Couchbase;

    public static class OddFeedDataCachingOperator
    {
        public static OddsData GetOddsData(IList<int> delayTimes)
        {
            var currentVersion = GetCurrentVersion();
            var fullChain = IsFullChain();

            var delayTimeDeadball = 0;
            var delayTimeLive = 0;
            var delayTimeOutright = 0;
            if (delayTimes.Count >= 3)
            {
                delayTimeDeadball = delayTimes[0];
                delayTimeLive = delayTimes[1];
                delayTimeOutright = delayTimes[2];
            }

            var cachePrefix = AppSettings.CachePrefix;
            var deadballRightVersion = GetRightVersion(delayTimeDeadball, currentVersion, fullChain);
            var liveRightVersion = GetRightVersion(delayTimeLive, currentVersion, fullChain);
            var outrightRightVersion = GetRightVersion(delayTimeOutright, currentVersion, fullChain);

            return new OddsData
            {
                DeadballMatches = GetMatches(CacheKeys.DeadballMatchKey(cachePrefix, deadballRightVersion), deadballRightVersion),
                LiveMatches = GetMatches(CacheKeys.LiveMatchKey(cachePrefix, liveRightVersion), liveRightVersion),
                Outrights = GetOutrights(CacheKeys.OutrightKey(cachePrefix, outrightRightVersion), outrightRightVersion)
            };
        }

        public static IList<Match> GetOutRight(int version)
        {
            using (var cache = new CouchbaseClient())
            {
                return cache.Get(CacheKeys.DeadballMatchKey(AppSettings.CachePrefix, version)) as IList<Match>;
            }
        }

        public static IList<Outright> GetMatchs(int version)
        {
            using (var cache = new CouchbaseClient())
            {
                return cache.Get(CacheKeys.OutrightKey(AppSettings.CachePrefix, version)) as IList<Outright>;
            }
        }

        public static IList<BetTypeSchema> GetBetTypeSchemas()
        {
            using (var cache = new CouchbaseClient())
            {
                var schemas = cache.Get<IList<BetTypeSchema>>(AppSettings.BetTypeSchemaKey);

                return schemas == null ? new List<BetTypeSchema>() : schemas;
            }
        }

        public static int GetCurrentVersion()
        {
            using (var cache = new CouchbaseClient())
            {
                return cache.Get<int>(CacheKeys.CurrentKey(AppSettings.CachePrefix));
            }
        }

        public static bool IsFullChain()
        {
            using (var cache = new CouchbaseClient())
            {
                return cache.Get<bool>(CacheKeys.FullChainKey(AppSettings.CachePrefix));
            }
        }

        public static int GetRightVersion(int delayTime, int currentVersion, bool isFullChain)
        {
            var rightVersion = currentVersion - (delayTime / (AppSettings.Interval / 1000));

            if (isFullChain)
            {
                return rightVersion > 0 ? rightVersion : AppSettings.TotalVersion - Math.Abs(rightVersion);
            }

            return rightVersion > -1 ? rightVersion : -1;
        }

        private static IList<Match> GetMatches(string key, int rightVersion)
        {
            if (rightVersion == -1)
            {
                return new List<Match>();
            }
            using (var cache = new CouchbaseClient())
            {
                var temp = cache.Get<IEnumerable<Match>>(key);
                return temp != null ? temp.ToList() : new List<Match>();
            }
        }

        private static IList<Outright> GetOutrights(string key, int rightVersion)
        {
            if (rightVersion == -1)
            {
                return new List<Outright>();
            }
            using (var cache = new CouchbaseClient())
            {
                var temp = cache.Get<IEnumerable<Outright>>(key);
                return temp != null ? temp.ToList() : new List<Outright>();

            }
        }
    }
}