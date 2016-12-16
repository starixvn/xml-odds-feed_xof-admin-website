namespace Fanex.OddsFeed.Common.Configurations
{
    public static class CacheKeys
    {
        public static string LiveMatchKey(string cachePrefix, int version) => $"{cachePrefix}livematchs{version}";

        public static string DeadballMatchKey(string cachePrefix, int version) => $"{cachePrefix}deadballmatches{version}";

        public static string OutrightKey(string cachePrefix, int version) => $"{cachePrefix}events{version}";

        public static string CurrentKey(string cachePrefix) => $"{cachePrefix}current";

        public static string FullChainKey(string cachePrefix) => $"{cachePrefix}fullchain";
    }
}
