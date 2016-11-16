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

    public static class Caching
    {
        private static readonly CouchbaseClient _cacheClient;

        static Caching()
        {
            _cacheClient = CouchbaseManager.Instance;
        }

        public static object Get(string key)
        {
            return _cacheClient.Get(key);
        }

        public static OddsData GetOddsData(IList<int> delayTimes)
        {
            var currentVersion = GetCurrentVersion();
            var fullChain = IsFullChain();

            int delayTimeDeadball = 0, delayTimeLive = 0, delayTimeOutright = 0;
            if (delayTimes.Count >= 3)
            {
                delayTimeDeadball = delayTimes[0];
                delayTimeLive = delayTimes[1];
                delayTimeOutright = delayTimes[2];
            }

            var cachePrefix = AppSettings.CachePrefix;
            int deadballRightVersion = GetRightVersion(delayTimeDeadball, currentVersion, fullChain);
            int liveRightVersion = GetRightVersion(delayTimeLive, currentVersion, fullChain);
            int outrightRightVersion = GetRightVersion(delayTimeOutright, currentVersion, fullChain);

            return new OddsData
            {
                DeadballMatches = GetMatches(string.Format("{0}deadballmatches{1}", cachePrefix, deadballRightVersion), deadballRightVersion),
                LiveMatches = GetMatches(string.Format("{0}livematchs{1}", cachePrefix, liveRightVersion), liveRightVersion),
                Outrights = GetOutrights(string.Format("{0}events{1}", cachePrefix, outrightRightVersion), outrightRightVersion)
            };
        }

        public static IList<BetTypeSchema> GetBetTypeSchemas()
        {
            var bettypeSchemas = Get(AppSettings.BetTypeSchemaKey) as IList<BetTypeSchema>;

            if (bettypeSchemas == null)
            {
                bettypeSchemas = JsonHelper.GetBetTypeSchemas(AppSettings.BetTypeSchemaPath);

                _cacheClient.Store(StoreMode.Set, AppSettings.BetTypeSchemaKey, bettypeSchemas);
            }

            return bettypeSchemas;
        }

        public static int GetCurrentVersion()
        {
            return _cacheClient.Get<int>(AppSettings.CachePrefix + "current");
        }

        public static bool IsFullChain()
        {
            return _cacheClient.Get<bool>(AppSettings.CachePrefix + "fullchain");
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
