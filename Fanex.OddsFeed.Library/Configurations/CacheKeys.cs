namespace Fanex.OddsFeed.Library.Configurations
{
    public static class CacheKeys
    {
        public static string LiveMatchKey(string cachePrefix, int version)
        {
            return string.Format("{0}livematchs{1}", cachePrefix, version);
        }

        public static string DeadballMatchKey(string cachePrefix, int version)
        {
            return string.Format("{0}deadballmatches{1}", cachePrefix, version);
        }

        public static string OutrightKey(string cachePrefix, int version)
        {
            return string.Format("{0}events{1}", cachePrefix, version);
        }

        public static string CurrentKey(string cachePrefix)
        {
            return string.Format("{0}current", cachePrefix);
        }

        public static string FullChainKey(string cachePrefix)
        {
            return string.Format("{0}fullchain", cachePrefix);
        }
    }
}
