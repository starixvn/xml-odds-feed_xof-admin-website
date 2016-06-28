// --------------------------------------------------------------------------
// <copyright file="Caching.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Library
{
    using System.Collections.Generic;
    using System.Configuration;
    using Couchbase;
    using Fanex.OddsFeed.Library.Matchs;
    using Fanex.OddsFeed.Library.Outrights;

    /// <summary>
    /// Class Caching.
    /// </summary>
    public static class Caching
    {        
        private static readonly CouchbaseClient _Cache;

        /// <summary>
        /// Initializes static members of the <see cref="Caching"/> class.
        /// </summary>
        static Caching()
        {
            _Cache = CouchbaseManager.Instance;
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="delayTimes">The delay times.</param>
        /// <returns>Object OddsDataToCache.</returns>
        public static OddsDataToCache GetData(IList<int> delayTimes)
        {
            var currentVersion = GetCurrentVersion();
            var fullChain = IsFullChain();

            var interval = int.Parse(ConfigurationManager.AppSettings.Get("Interval")) / 1000;
            OddsDataToCache result = new OddsDataToCache();
            int delayTimeDeadball = 0, delayTimeLive = 0, delayTimeOutright = 0;
            if (delayTimes.Count >= 3)
            {
                delayTimeDeadball = delayTimes[0];
                delayTimeLive = delayTimes[1];
                delayTimeOutright = delayTimes[2];
            }

            int rightVersion;
            var prefix = ConfigurationManager.AppSettings["Cache.Prefix"];
            ////Get Deadball
            rightVersion = GetRightVersion(delayTimeDeadball, currentVersion, fullChain);
            result.DeadballMatches = rightVersion == -1
                ? null
                : _Cache.Get<IList<Match>>(prefix + "deadballmatches" + rightVersion);

            ////Get Live
            rightVersion = GetRightVersion(delayTimeLive, currentVersion, fullChain);
            result.LiveMatches = rightVersion == -1
                ? null
                : _Cache.Get<IList<Match>>(prefix + "livematchs" + rightVersion);

            ////Get Outright
            rightVersion = GetRightVersion(delayTimeOutright, currentVersion, fullChain);
            result.Events = rightVersion == -1
                ? null
                : _Cache.Get<IList<Event>>(prefix + "events" + rightVersion);

            return result;
        }

        public static object Get(string key)
        {
            return _Cache.Get(key);
        }

        public static int GetCurrentVersion()
        {
            return _Cache.Get<int>(ConfigurationManager.AppSettings["Cache.Prefix"] + "current");
        }

        public static bool IsFullChain()
        {
            return _Cache.Get<bool>(ConfigurationManager.AppSettings["Cache.Prefix"] + "fullchain");
        }

        /// <summary>
        /// Gets the right version.
        /// </summary>
        /// <param name="delayTime">The delay time.</param>
        /// <param name="currentVersion">The current version.</param>
        /// <param name="isFullChain">If set to <c>true</c> [is full chain].</param>
        /// <returns>System integer.</returns>
        public static int GetRightVersion(int delayTime, int currentVersion, bool isFullChain)
        {
            var interval = int.Parse(ConfigurationManager.AppSettings.Get("Interval")) / 1000;
            var a = currentVersion - (delayTime / interval);
            if (isFullChain)
            {
                return
                    a > 0
                    ? a
                    : int.Parse(ConfigurationManager.AppSettings.Get("TotalVersion")) - System.Math.Abs(a);
            }
            else
            {
                return
                    a > -1
                    ? a
                    : -1;
            }
        }
    }
}
