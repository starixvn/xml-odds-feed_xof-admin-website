namespace Fanex.OddsFeed.WindowsServiceLibrary.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
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
            var line = 0;
            var result = false;

            try
            {
                var liveOdds = DbAccess.GetLiveOddsData();
                line++;
                var deadballOdds = DbAccess.GetDeadballOddsData();
                line++;
                var outrightOdds = DbAccess.GetOutrightOddsData();
                line++;
                var liveOddsFlexs = GetOddFlexs(liveOdds.ResultSet.Tables[1]);
                line++;
                var deadballOddsFlexs = GetOddFlexs(deadballOdds.ResultSet.Tables[1]);
                line++;
                var betTypeSchemas = GetBetTypeSchemas();

                line++;
                var liveMatches = _matchMapping.GetMatches(liveOdds.ResultSet.Tables[0], betTypeSchemas, true, liveOddsFlexs);
                line++;
                var deadBallMatches = _matchMapping.GetMatches(deadballOdds.ResultSet.Tables[0], betTypeSchemas, false, deadballOddsFlexs);
                line++;
                var outrights = _outrightMapping.GetOutrights(outrightOdds.ResultSet.Tables[0]);
                line++;

                result = StoreData(CacheKeys.CurrentKey(_cachePrefix), currentVersion, _numberTries);
                line++;
                result = result && StoreData(CacheKeys.LiveMatchKey(_cachePrefix, currentVersion), liveMatches, _numberTries);
                line++;
                result = result && StoreData(CacheKeys.DeadballMatchKey(_cachePrefix, currentVersion), deadBallMatches, _numberTries);
                line++;
                result = result && StoreData(CacheKeys.OutrightKey(_cachePrefix, currentVersion), outrights, _numberTries);

                return result;
            }
            catch (Exception exception)
            {
                var innerExceptionMessage = exception.InnerException == null ? "No inner" : exception.InnerException.Message;
                var message = string.Format("Error at line: {0}. Main error: {1}. Inner error:{2} - stackstrace:{3}",
                    line, exception.Message, innerExceptionMessage, exception.StackTrace);

                LogService.Error(message, exception);
            }

            return true;
        }

        private static IList<OddFlex> GetOddFlexs(DataTable table)
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
                LogService.Error(string.Format("Couchbase error! --- Interrupted while trying to set.  Exception:{0}", exception.Message), exception);
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

                        LogService.Error(string.Format("Couchbase error! --- Backing off, tries so far: {0}", backOffExpired));
                    }
                    else
                    {
                        LogService.Error("Couchbase error!");
                    }

                    backOffExpired++;
                } while (backOffExpired < numberTries);

                if (!result && (backOffExpired > numberTries))
                {
                    throw new ApplicationException(string.Format("Could not perform a set after {0} tries.", numberTries));
                }

                return result;
            }
        }
    }
}