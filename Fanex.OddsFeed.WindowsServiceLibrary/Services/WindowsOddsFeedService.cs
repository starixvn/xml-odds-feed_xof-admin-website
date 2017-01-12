namespace Fanex.OddsFeed.WindowsServiceLibrary.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using Common;
    using Common.Configurations;
    using Common.DTO;
    using Common.Helpers;
    using Common.Schemas;
    using Couchbase;
    using Enyim.Caching.Memcached;
    using Logging;
    using Mapping;

    public class WindowsOddsFeedService : IWindowsOddsFeedService
    {
        private readonly int _numberTries;
        private readonly string _cachePrefix;
        private readonly IMatchMapping _matchMapping;
        private readonly IOutrightMapping _outrightMapping;

        public WindowsOddsFeedService()
        {
            _numberTries = AppSettings.NumberTries;
            _cachePrefix = AppSettings.CachePrefix;
            _matchMapping = new MatchMapping();
            _outrightMapping = new OutrightMapping();
        }

        public TimeSpan FullTime { get; set; }

        public TimeSpan LiveballTime { get; set; }

        public TimeSpan DeadballTime { get; set; }

        public TimeSpan OutrightTime { get; set; }

        public void Reset()
        {
            StoreData(AppSettings.BetTypeSchemaKey, new List<BetTypeSchema>(), 0);
            StoreData(CacheKeys.CurrentKey(_cachePrefix), 0, _numberTries);
            StoreData(CacheKeys.FullChainKey(_cachePrefix), false, _numberTries);
        }

        public bool StoreFullChain()
        {
            return StoreData(CacheKeys.FullChainKey(_cachePrefix), true, _numberTries);
        }

        public bool Update(int currentVersion)
        {
            var result = false;

            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                var betTypeSchemas = GetBetTypeSchemas();
                var liveMatches = GetLiveMatches(betTypeSchemas);
                var deadBallMatches = GetDeadballMatches(betTypeSchemas);
                var outrights = GetOutrights();

                result = StoreData(CacheKeys.CurrentKey(_cachePrefix), currentVersion, _numberTries);
                result = result && StoreData(CacheKeys.LiveMatchKey(_cachePrefix, currentVersion), liveMatches, _numberTries);
                result = result && StoreData(CacheKeys.DeadballMatchKey(_cachePrefix, currentVersion), deadBallMatches, _numberTries);
                result = result && StoreData(CacheKeys.OutrightKey(_cachePrefix, currentVersion), outrights, _numberTries);

                stopwatch.Stop();

                FullTime = stopwatch.Elapsed;

                return result;
            }
            catch (Exception exception)
            {
                var innerExceptionMessage = exception.InnerException == null ? "No inner" : exception.InnerException.Message;
                var message = $"Main error: {exception.Message}. Inner error:{innerExceptionMessage} - stackstrace:{exception.StackTrace}";

                LogService.Error(message, exception);
            }

            return true;
        }

        private IEnumerable<Match> GetLiveMatches(IList<BetTypeSchema> betTypeSchemas) 
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            using (var liveOdds = DbAccess.GetLiveOddsData())
            {
                stopwatch.Stop();
                LiveballTime = stopwatch.Elapsed;

                return _matchMapping.GetMatches(liveOdds.Tables[0], betTypeSchemas, true, GetOddsFlexGroups(liveOdds.Tables[1]));
            }
        }

        private IEnumerable<Match> GetDeadballMatches(IList<BetTypeSchema> betTypeSchemas)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            using (var deadballOdds = DbAccess.GetDeadballOddsData())
            {
                stopwatch.Stop();
                DeadballTime = stopwatch.Elapsed;

                return _matchMapping.GetMatches(deadballOdds.Tables[0], betTypeSchemas, false, GetOddsFlexGroups(deadballOdds.Tables[1]));
            }
        }

        private IEnumerable<Outright> GetOutrights()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            using (var outrightOdds = DbAccess.GetOutrightOddsData())
            {
                stopwatch.Stop();
                OutrightTime = stopwatch.Elapsed;

                return _outrightMapping.GetOutrights(outrightOdds);
            }
        }

        private static IEnumerable<OddFlex> GetOddFlexs(DataTable table)
        {
            return table.AsEnumerable()
                .Select(
                row => new OddFlex
                {
                    BetChoice = row["betChoice"].ToString(),
                    BetType = Convert.ToInt32(row["bettype"]),
                    OddId = Convert.ToInt32(row["oddsid"]),
                    Odds = Convert.ToDecimal(row["odds"]),
                    OddsInfo = row["OddsInfo"].ToString()
                }).ToList();
        }

        private static IEnumerable<OddFlexGroup> GetOddsFlexGroups(DataTable table)
        {
            var oddsFlexGroups = new List<OddFlexGroup>();

            foreach (DataRow row in table.Rows)
            {
                var oddsId = Convert.ToInt32(row["oddsid"]);
                var oddsFlex = new OddFlex
                {
                    BetChoice = row["betChoice"].ToString(),
                    BetType = Convert.ToInt32(row["bettype"]),
                    OddId = oddsId,
                    Odds = Convert.ToDecimal(row["odds"]),
                    OddsInfo = row["OddsInfo"].ToString()
                };
                var oddsFlexGroup = oddsFlexGroups.FirstOrDefault(odds => odds.OddsId == oddsId);

                if (oddsFlexGroup == null)
                {
                    oddsFlexGroups.Add(new OddFlexGroup
                    {
                        OddsId = oddsId,
                        OddsFlexs = new List<OddFlex> { oddsFlex }
                    });
                }
                else
                {
                    oddsFlexGroup.OddsFlexs.Add(oddsFlex);
                }
            }

            return oddsFlexGroups;
        }

        public static IList<BetTypeSchema> GetBetTypeSchemas()
        {
            using (var cache = new CouchbaseClient())
            {
                var schemas = cache.Get<IList<BetTypeSchema>>(AppSettings.BetTypeSchemaKey);

                if (schemas == null || schemas.Count == 0)
                {
                    schemas = JsonHelper.GetBetTypeSchemas(AppSettings.BetTypeSchemaPath);

                    if (schemas != null)
                    {
                        cache.Store(StoreMode.Set, AppSettings.BetTypeSchemaKey, schemas);
                    }
                }

                return schemas;
            }
        }

        private static bool StoreData(string key, object data, int numberTries)
        {
            var result = false;

            try
            {
                if (data != null)
                {
                    result = TryStore(key, data, numberTries);
                }
            }
            catch (Exception exception)
            {
                LogService.Error($"Couchbase error! --- Interrupted while trying to set.  Exception:{exception.Message}", exception);
            }

            return result;
        }

        private static bool TryStore(string key, object data, int numberTries)
        {
            var result = false;
            var backOffExpired = 0;

            using (var cacheClient = new CouchbaseClient())
            {
                do
                {
                    result = cacheClient.Store(StoreMode.Set, key, data);

                    if (result)
                    {
                        break;
                    }

                    if (backOffExpired > 0)
                    {
                        var backOffMilliseconds = Math.Min(1000, Math.Pow(2, backOffExpired));

                        Thread.Sleep((int)backOffMilliseconds);

                        LogService.Error($"Couchbase error! --- Backing off, tries so far: {backOffExpired}");
                    }
                    else
                    {
                        LogService.Error("Couchbase error!");
                    }

                    backOffExpired++;

                } while (backOffExpired < numberTries);

                if (!result && (backOffExpired > numberTries))
                {
                    throw new ApplicationException($"Could not perform a set after {numberTries} tries.");
                }

                return result;
            }
        }
    }
}