namespace Fanex.OddsFeed.Library
{
    using System;
    using System.Collections.Generic;
    using Configurations;
    using Couchbase;
    using Enyim.Caching.Memcached;
    using Helper;
    using Model;
    using Nodes;
    using Schemas;

    public static class OddFeedDataCachingOperator
    {
        private static readonly CouchbaseClient _cacheClient;

        static OddFeedDataCachingOperator()
        {
            _cacheClient = CouchbaseManager.Instance;
        }


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
            int liveRightVersion = GetRightVersion(delayTimeLive, currentVersion, fullChain);
            int outrightRightVersion = GetRightVersion(delayTimeOutright, currentVersion, fullChain);

            return new OddsData
            {
                DeadballMatches = GetMatches(CacheKeys.DeadballMatchKey(cachePrefix, deadballRightVersion), deadballRightVersion),
                LiveMatches = GetMatches(CacheKeys.LiveMatchKey(cachePrefix, liveRightVersion), liveRightVersion),
                Outrights = GetOutrights(CacheKeys.OutrightKey(cachePrefix, outrightRightVersion), outrightRightVersion)
            };
        }

        public static IList<Match> GetOutRight(int version)
        {
           return _cacheClient.Get(CacheKeys.DeadballMatchKey(AppSettings.CachePrefix, version)) as IList<Match>;
        }

        public static IList<Outright> GetMatchs(int version)
        {
            return _cacheClient.Get(CacheKeys.OutrightKey(AppSettings.CachePrefix, version)) as IList<Outright>;
        }

        public static IList<BetTypeSchema> GetBetTypeSchemas()
        {
            var bettypeSchemas = _cacheClient.Get(AppSettings.BetTypeSchemaKey) as IList<BetTypeSchema>;

            if (bettypeSchemas == null)
            {
                bettypeSchemas = JsonHelper.GetBetTypeSchemas(AppSettings.BetTypeSchemaPath);

                _cacheClient.Store(StoreMode.Set, AppSettings.BetTypeSchemaKey, bettypeSchemas);
            }

            return bettypeSchemas;
        }

        public static int GetCurrentVersion()
        {
            return _cacheClient.Get<int>(CacheKeys.CurrentKey(AppSettings.CachePrefix));
        }

        public static bool IsFullChain()
        {
            return _cacheClient.Get<bool>(CacheKeys.FullChainKey(AppSettings.CachePrefix));
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
            return rightVersion == -1
                ? new List<Match>()
                : _cacheClient.Get<List<Match>>(key);
        }

        private static IList<Outright> GetOutrights(string key, int rightVersion)
        {
            return rightVersion == -1
                ? new List<Outright>()
                : _cacheClient.Get<List<Outright>>(key);
        }
    }
}
