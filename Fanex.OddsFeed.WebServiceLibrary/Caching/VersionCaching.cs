namespace Fanex.OddsFeed.WebServiceLibrary.Caching
{
    using Couchbase.Core;
    using Fanex.OddsFeed.Common.Configurations;
    using Fanex.OddsFeed.Common.DTO;
    using Fanex.OddsFeed.WebServiceLibrary.Models;
    using System;

    public class VersionCaching : IVersionCaching
    {
        private readonly string _oddsFeedBucket;
        private readonly string _currentVersionKey;
        private readonly string _isFullChainKey;

        public VersionCaching()
        {
            _oddsFeedBucket = AppSettings.BucketOddsFeed;
            _currentVersionKey = CacheKeys.CurrentKey(AppSettings.CachePrefix);
            _isFullChainKey = CacheKeys.FullChainKey(AppSettings.CachePrefix);
        }

        public CacheVersion GetRightVersion(ICluster cluster, OddsFeedsModel model)
        {
            var currentVersion = GetCurrentVersion(cluster);
            var isFullChain = IsFullChain(cluster);

            var delayTimeDeadball = 0;
            var delayTimeLive = 0;
            var delayTimeOutright = 0;
            if (model.UserProfile.DelayTimes.Count >= 3)
            {
                delayTimeDeadball = model.UserProfile.DelayTimes[0];
                delayTimeLive = model.UserProfile.DelayTimes[1];
                delayTimeOutright = model.UserProfile.DelayTimes[2];
            }

            return new CacheVersion
            {
                CurrentVersionOutright = GetRightVersion(delayTimeOutright, currentVersion, isFullChain),
                CurrentVersionLiveMatch = GetRightVersion(delayTimeLive, currentVersion, isFullChain),
                CurrentVersionDeadballMatch = GetRightVersion(delayTimeDeadball, currentVersion, isFullChain)
            };
        }

        private static int GetRightVersion(int delayTime, int currentVersion, bool isFullChain)
        {
            var rightVersion = currentVersion - (delayTime / AppSettings.Interval);

            if (isFullChain)
            {
                return rightVersion > -1 ? rightVersion : AppSettings.TotalVersion - Math.Abs(rightVersion);
            }

            return rightVersion > -1 ? rightVersion : -1;
        }

        private int GetCurrentVersion(ICluster cluster)
        {
            var currentVersion = 0;
            using (var bucket = cluster.OpenBucket(_oddsFeedBucket))
            {
                var get = bucket.GetDocument<int>(_currentVersionKey);
                currentVersion = get.Document.Content;
            }

            return currentVersion;
        }

        private bool IsFullChain(ICluster cluster)
        {
            var isFullChain = false;
            using (var bucket = cluster.OpenBucket(_oddsFeedBucket))
            {
                var get = bucket.GetDocument<bool>(_isFullChainKey);
                isFullChain = get.Document.Content;
            }

            return isFullChain;
        }
    }
}
