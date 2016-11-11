namespace Fanex.OddsFeed.Library.Services
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Threading;
    using Couchbase;
    using Enyim.Caching.Memcached;
    using Logging;
    using Mapping;
    using Model;

    public class WindowsOddsFeedService : IWindowsOddsFeedService
    {
        private readonly CouchbaseClient _cacheClient;
        private readonly int _numberTries;
        private readonly string _cachePrefix;

        private IMatchMapping _matchMapping;
        private IOutrightMapping _outrightMapping;

        public WindowsOddsFeedService()
        {
            _numberTries = Convert.ToInt32(ConfigurationManager.AppSettings["NumTries"]);
            _cachePrefix = ConfigurationManager.AppSettings["Cache.Prefix"].ToString();
            _cacheClient = CouchbaseManager.Instance;

            _matchMapping = new MatchMapping();
            _outrightMapping = new OutrightMapping();
        }

        protected string CurrentPrefix
        {
            get
            {
                return string.Format("{0}current", _cachePrefix);
            }
        }

        protected string FullChainPrefix
        {
            get
            {
                return string.Format("{0}fullchain", _cachePrefix);
            }
        }

        public bool Reset()
        {
            return StoreData(CurrentPrefix, 0, _numberTries)
                && StoreData(FullChainPrefix, false, _numberTries);
        }

        public bool StoreFullChain()
        {
            return StoreData(FullChainPrefix, true, _numberTries);
        }

        public bool Update(int currentVersion)
        {
            int line = 0;

            try
            {
                var liveOdds = DbConnect.GetLiveOddsData();
                line++;
                var deadballOdds = DbConnect.GetDeadballOddsData();
                line++;
                var outrightOdds = DbConnect.GetOutrightOddsData();
                line++;
                var liveOddsFlexs = GetOddFlexs(liveOdds.ResultSet.Tables[1]);
                line++;
                var deadballOddsFlexs = GetOddFlexs(deadballOdds.ResultSet.Tables[1]);
                line++;

                var liveMatches = _matchMapping.GetMatchNodes(liveOdds.ResultSet.Tables[0], null, true, liveOddsFlexs);
                line++;
                var deadBallMatches = _matchMapping.GetMatchNodes(deadballOdds.ResultSet.Tables[0], null, false, deadballOddsFlexs);
                line++;
                var outrights = _outrightMapping.GetOutrightNodes(outrightOdds.ResultSet.Tables[0]);
                line++;

                bool result = StoreData(CurrentPrefix, 0, _numberTries);
                line++;
                result = result && StoreData(string.Format("{0}livematchs{1}", _cachePrefix, currentVersion), liveMatches, _numberTries);
                line++;
                result = result && StoreData(string.Format("{0}deadballmatches{1}", _cachePrefix, currentVersion), deadBallMatches, _numberTries);
                line++;
                result = result && StoreData(string.Format("{0}events{1}", _cachePrefix, currentVersion), outrights, _numberTries);

                return result;
            }
            catch (Exception exception)
            {
                var innerExceptionMessage = exception.InnerException == null ? "No inner" : exception.InnerException.Message;
                var message = string.Format("Error at line: {0}. Main error: {1}. Inner error:{2}", line, exception.Message, innerExceptionMessage);

                LogService.Error(message);
            }

            return true;
        }

        private IList<OddFlex> GetOddFlexs(DataTable table)
        {
            return table.AsEnumerable()
                .Select(
                row => new OddFlex()
                {
                    BetChoice = row["betChoice"].ToString(),
                    BetType = Convert.ToInt32(row["bettype"]),
                    OddId = Convert.ToInt32(row["oddsid"]),
                    Odds = Convert.ToInt32(row["odds"]),
                    OddsInfo = row["OddsInfo"].ToString()
                }).ToList();
        }

        private bool StoreData(string key, object data, int numberTries)
        {
            bool result = false;

            try
            {
                result = TryStore(key, data, numberTries);
            }
            catch (Exception exception)
            {
                LogService.Error(string.Format("Couchbase error! --- Interrupted while trying to set.  Exception:{0}", exception.Message));
            }
            
            return result;
        }

        private bool TryStore(string key, object data, int numberTries)
        {
            bool result = false;
            int backOffExpired = 0;

            do
            {
                result = _cacheClient.Store(StoreMode.Set, key, data);

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
