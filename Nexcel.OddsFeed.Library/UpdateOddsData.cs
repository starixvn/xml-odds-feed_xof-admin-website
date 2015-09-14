// --------------------------------------------------------------------------
// <copyright file="UpdateOddsData.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Nexcel.OddsFeed.Library
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Threading;
    using Couchbase;
    using Enyim.Caching.Memcached;
    using Enyim.Caching.Memcached.Results;
    using Fanex.Common;
    using Fanex.Logging;
    using Nexcel.OddsFeed.Library.Matches;
    using Nexcel.OddsFeed.Library.Matchs;
    using Nexcel.OddsFeed.Library.Outrights;

    /// <summary>
    /// Class UpdateOddsData.
    /// </summary>
    public static class UpdateOddsData
    {
        private static readonly CouchbaseClient _Cache;
        private static readonly int _NumTries = int.Parse(ConfigurationManager.AppSettings["NumTries"].ToString());
        private static readonly string _Prefix = ConfigurationManager.AppSettings["Cache.Prefix"].ToString();

        /// <summary>
        /// Initializes static members of the <see cref="UpdateOddsData"/> class.
        /// </summary>
        static UpdateOddsData()
        {
            _Cache = CouchbaseManager.Instance;           
        }

        public static bool Reset()
        {
            bool updateSuccessful = false;

            ////update cache
            updateSuccessful = Set(_Prefix + "current", 0, _NumTries);
            updateSuccessful = updateSuccessful && Set(_Prefix + "fullchain", false, _NumTries);

            return updateSuccessful;
        }    

        public static bool FullChain()
        {
            return Set(_Prefix + "fullchain", true, _NumTries);
        }

        /// <summary>
        /// Updates the specified current version.
        /// </summary>
        /// <param name="currentVersion">The current version.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool Update(int currentVersion)
        {
            bool updateSuccessful = false;

            var liveodds = DbConnect.GetLiveOddsData();
            var deadballOdds = DbConnect.GetDeadballOddsData();
            var outrightOdds = DbConnect.GetOutrightOddsData();

            IList<Match> liveMatches;
            IList<Match> deadballMatches;
            IList<Event> events;
            liveMatches = new List<Match>();
            deadballMatches = new List<Match>();
            events = new List<Event>();

            ////live
            BuildMatchOdds(liveodds.ResultSet.Tables[0], true, liveMatches);
            ////deadball
            BuildMatchOdds(deadballOdds.ResultSet.Tables[0], false, deadballMatches);
            ////outrights
            BuildOutRightOdds(outrightOdds.ResultSet.Tables[0], events);
            
            ////update cache
            updateSuccessful = Set(_Prefix + "current", currentVersion, _NumTries);
            updateSuccessful = updateSuccessful && Set(_Prefix + "livematchs" + currentVersion, liveMatches, _NumTries);
            updateSuccessful = updateSuccessful && Set(_Prefix + "deadballmatches" + currentVersion, deadballMatches, _NumTries);
            updateSuccessful = updateSuccessful && Set(_Prefix + "events" + currentVersion, events, _NumTries);

            return updateSuccessful;
        }

        #region Private methods

        /// <summary>
        /// Builds the match odds.
        /// </summary>
        /// <param name="dataTable">The data table.</param>
        /// <param name="isLive">If set to <c>true</c> [is live].</param>
        /// <param name="matches">The matches.</param>
        private static void BuildMatchOdds(DataTable dataTable, bool isLive, IList<Match> matches)
        {            
            var match = new Match();
            var matchId = 0;
            foreach (DataRow item in dataTable.Rows)
            {                
                ////bookies
                var bookie = new Bookie();
                var bookieName = CreateBookieName();
                bookie.Name = bookieName;
                ////odds                
                var odd = CreateMatchOdd(item);

                if (odd != null)
                {
                    odd.TimeStamp = Converter.ToInt(item["TimeStamp"].ToString());                    
                    odd.BetTypeId = int.Parse(item["BetType"].ToString());
                    bookie.Odds.Add(odd);
                }

                CreateMatch(isLive, matches, ref match, ref matchId, item, bookie, odd);
            }
        }

        /// <summary>
        /// Builds the out right odds.
        /// </summary>
        /// <param name="dataTable">The data table.</param>
        /// <param name="events">The events.</param>
        private static void BuildOutRightOdds(DataTable dataTable, IList<Event> events)
        {
            ////event
            var e = new Event();
            var leagueId = 0;
            foreach (DataRow item in dataTable.Rows)
            {
                ////build odds
                ////odds
                ////odds-team
                var team = CreateTeam(item);

                var odds = new List<OutrightsOdd>();
                var odd = new OutrightsOdd
                {
                    Id = int.Parse(item["OrID"].ToString()),
                    Team = team,
                    Value = item["Odds"].ToString()
                };

                odds.Add(odd);

                if (leagueId != int.Parse(item["LeagueID"].ToString()))
                {
                    leagueId = int.Parse(item["LeagueID"].ToString());
                    ////we add new event
                    ////content
                    IList<Language> content = CreateLeague(item);

                    e = new Event
                    {
                        Id = leagueId,
                        CountryCode = item["CountryCode"].ToString(),
                        Content = content,
                        Odds = odds,
                        Sport = item["LeagueNameTC"].ToString(),
                        SportId = int.Parse(item["SportID"].ToString()),
                        Time = DateTime.Parse(item["EventDate"].ToString())
                    };

                    events.Add(e);
                }
                else
                {
                    ////add odd only
                    e.Odds.Add(odd);
                }
            }
        }

        /// <summary>
        /// Creates the match.
        /// </summary>
        /// <param name="isLive">If set to <c>true</c> [is live].</param>
        /// <param name="matches">The matches.</param>
        /// <param name="match">The match.</param>
        /// <param name="matchId">The match identifier.</param>
        /// <param name="item">The data row.</param>
        /// <param name="bookie">The bookie.</param>
        /// <param name="odd">The MatchOdd.</param>
        private static void CreateMatch(bool isLive, IList<Match> matches, ref Match match, ref int matchId, DataRow item, Bookie bookie, MatchOdd odd)
        {
            if (matchId != int.Parse(item["MatchID"].ToString()))
            {
                matchId = int.Parse(item["MatchID"].ToString());
                ////league]
                var league = CreateLeague(item);
                ////home
                var home = CreateHome(item);
                ////away
                var away = CreateAway(item);
                match = new Match
                {
                    Id = matchId,
                    LeagueId = int.Parse(item["LeagueID"].ToString()),
                    CountryCode = item["CountryCode"].ToString(),
                    League = league,
                    Home = home,
                    Away = away,
                    Sport = item["SportTypeName"].ToString(),
                    SportId = int.Parse(item["SportType"].ToString()),
                    Time = DateTime.Parse(item["KickOffTime"].ToString()),
                    Running = isLive
                                    ? true
                                    : matches.Where(i => i.IsLive && i.Id == int.Parse(item["MatchID"].ToString()))
                                    .FirstOrDefault() != null,
                    ////item["EventStatus"].ToString() == "running" ? true : false,
                    Bookie = bookie,
                    BetType = int.Parse(item["BetType"].ToString()),
                    IsLive = isLive,
                    TimeStamp = Converter.ToInt(item["TimeStamp"].ToString())
                };

                matches.Add(match);
            }
            else
            {
                ////add odd only
                if (odd != null)
                {
                    if (odd.TimeStamp > match.TimeStamp)
                    {
                        match.TimeStamp = odd.TimeStamp;
                    }

                    match.Bookie.Odds.Add(odd);
                }
            }
        }

        /// <summary>
        /// Creates the match odd.
        /// </summary>
        /// <param name="item">The data row.</param>
        /// <returns>Object MatchOdd.</returns>
        private static MatchOdd CreateMatchOdd(DataRow item)
        {            
            switch (int.Parse(item["BetType"].ToString()))
            {
                case 1:
                    return CreateMatchOddBettype1(item);                    
                case 2:
                    ////Odd/Even
                    return CreateMatchOddBettype2(item);                    
                case 3:
                    return CreateMatchOddBettype3(item);
                case 4:
                    ////Correct Score
                    return CreateMatchOddBettype4(item);                    
                case 5:
                    return CreateMatchOddBettype5(item);
                case 6:
                    ////Total Goal
                    return CreateMatchOddBettype6(item);                    
                case 7:
                    return CreateMatchOddBettype7(item);
                case 8:
                    return CreateMatchOddBettype8(item);
                case 12:
                    ////First-Half Odd/Even
                    return CreateMatchOddBettype12(item);                    
                case 14:
                    ////first-last
                    return CreateMatchOddBettype14(item);                    
                case 15:
                    return CreateMatchOddBettype15(item);                    
                case 16:
                    ////HT/FT
                    return CreateMatchOddBettype16(item);
                case 20:
                    ////HT/FT
                    return CreateMatchOddBettype20(item);                   
                default:
                    return null;                    
            }            
        }

        #region Match Odd Bettype

        /// <summary>
        /// Creates the match odd BETTYPE16.
        /// </summary>
        /// <param name="item">The data row.</param>
        /// <returns>Object MatchOdd.</returns>
        private static MatchOdd CreateMatchOddBettype20(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "ml",
                O1 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["Odds1A"]))),
                O2 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["Odds2A"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        /// <summary>
        /// Creates the match odd BETTYPE16.
        /// </summary>
        /// <param name="item">The data row.</param>
        /// <returns>Object MatchOdd.</returns>
        private static MatchOdd CreateMatchOddBettype16(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "htft",
                HH = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs11"]))),
                HD = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs10"]))),
                HA = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs12"]))),
                DH = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs01"]))),
                DD = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs00"]))),
                DA = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs02"]))),
                AH = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs21"]))),
                AD = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs20"]))),
                AA = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs22"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        /// <summary>
        /// Creates the match odd BETTYPE15.
        /// </summary>
        /// <param name="item">The data row.</param>
        /// <returns>Object MatchOdd.</returns>
        private static MatchOdd CreateMatchOddBettype15(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "st_1",
                O1 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["Com1"]))),
                O2 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["ComX"]))),
                O3 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["Com2"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        /// <summary>
        /// Creates the match odd BETTYPE14.
        /// </summary>
        /// <param name="item">The data row.</param>
        /// <returns>Object MatchOdd.</returns>
        private static MatchOdd CreateMatchOddBettype14(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "fl",
                HF = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs11"]))),
                HL = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs12"]))),
                AF = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs21"]))),
                AL = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs22"]))),
                NG = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs00"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        /// <summary>
        /// Creates the match odd BETTYPE12.
        /// </summary>
        /// <param name="item">The data row.</param>
        /// <returns>Object MatchOdd.</returns>
        private static MatchOdd CreateMatchOddBettype12(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "oe_1",
                O = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["Odds1A"]))),
                E = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["Odds2A"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        /// <summary>
        /// Creates the match odd BETTYPE8.
        /// </summary>
        /// <param name="item">The data row.</param>
        /// <returns>Object MatchOdd.</returns>
        private static MatchOdd CreateMatchOddBettype8(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "ou_1",
                H = string.Format("{0:0.#########}", Converter.ToDecimal(item["Hdp1"])),
                O = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["Odds1A"]))),
                U = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["Odds2A"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        /// <summary>
        /// Creates the match odd BETTYPE7.
        /// </summary>
        /// <param name="item">The data row.</param>
        /// <returns>Object MatchOdd.</returns>
        private static MatchOdd CreateMatchOddBettype7(DataRow item)
        {
            var h = Converter.ToDecimal(item["Hdp2"]) - Converter.ToDecimal(item["Hdp1"]);
            var odds2A_7 = Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["Odds2A"]));
            var odds1A_7 = Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["Odds1A"]));
            var odd = new MatchOdd
            {
                Type = "ah_1",
                H = string.Format("{0:0.#########}", h),
                O = string.Format("{0:0.#########}", h > 0 ? odds2A_7 : odds1A_7),
                U = string.Format("{0:0.#########}", h > 0 ? odds1A_7 : odds2A_7),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        /// <summary>
        /// Creates the match odd BETTYPE6.
        /// </summary>
        /// <param name="item">The data row.</param>
        /// <returns>Object MatchOdd.</returns>
        private static MatchOdd CreateMatchOddBettype6(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "tg",
                O1 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs00"]))),
                O2 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs01"]))),
                O3 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs10"]))),
                O4 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs11"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        /// <summary>
        /// Creates the match odd BETTYPE5.
        /// </summary>
        /// <param name="item">The data row.</param>
        /// <returns>Object MatchOdd.</returns>
        private static MatchOdd CreateMatchOddBettype5(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "st",
                O1 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["Com1"]))),
                O2 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["ComX"]))),
                O3 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["Com2"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        /// <summary>
        /// Creates the match odd BETTYPE4.
        /// </summary>
        /// <param name="item">The data row.</param>
        /// <returns>Object MatchOdd.</returns>
        private static MatchOdd CreateMatchOddBettype4(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "cs",
                C00 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs00"]))),
                C11 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs11"]))),
                C22 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs22"]))),
                C33 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs33"]))),
                C44 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs44"]))),

                C01 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs01"]))),
                C02 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs02"]))),
                C03 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs03"]))),
                C04 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs04"]))),
                C05 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs05"]))),

                C10 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs10"]))),
                C20 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs20"]))),
                C30 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs30"]))),
                C40 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs40"]))),
                C50 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs50"]))),

                C12 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs12"]))),
                C13 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs13"]))),
                C23 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs23"]))),
                C14 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs14"]))),
                C24 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs24"]))),
                C34 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs34"]))),

                C21 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs21"]))),
                C31 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs31"]))),
                C32 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs32"]))),
                C41 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs41"]))),
                C42 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs42"]))),
                C43 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs43"]))),

                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        /// <summary>
        /// Creates the match odd BETTYPE3.
        /// </summary>
        /// <param name="item">The data row.</param>
        /// <returns>Object MatchOdd.</returns>
        private static MatchOdd CreateMatchOddBettype3(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "ou",
                H = string.Format("{0:0.#########}", Converter.ToDecimal(item["Hdp1"])),
                O = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["Odds1A"]))),
                U = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["Odds2A"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        /// <summary>
        /// Creates the match odd BETTYPE2.
        /// </summary>
        /// <param name="item">The data row.</param>
        /// <returns>Object MatchOdd.</returns>
        private static MatchOdd CreateMatchOddBettype2(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "oe",
                O = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["Odds1A"]))),
                E = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["Odds2A"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        /// <summary>
        /// Creates the match odd BETTYPE1.
        /// </summary>
        /// <param name="item">The data row.</param>
        /// <returns>Object MatchOdd.</returns>
        private static MatchOdd CreateMatchOddBettype1(DataRow item)
        {
            var tmp = Converter.ToDecimal(item["Hdp2"]) - Converter.ToDecimal(item["Hdp1"]);
            var odds1A_1 = Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["Odds1A"]));
            var odds2A_1 = Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["Odds2A"]));
            var odd = new MatchOdd
            {
                Type = "ah",
                H = string.Format("{0:0.#########}", tmp),
                O = string.Format("{0:0.#########}", tmp > 0 ? odds2A_1 : odds1A_1),
                U = string.Format("{0:0.#########}", tmp > 0 ? odds1A_1 : odds2A_1),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        #endregion

        /// <summary>
        /// Creates the away.
        /// </summary>
        /// <param name="item">The data row.</param>
        /// <returns>List of object Language.</returns>
        private static List<Language> CreateAway(DataRow item)
        {
            var away = new List<Language>
                                   {
                                       new Language
                                           {
                                               Code = "en-US",
                                               Content = item["Away"].ToString()
                                           },
                                       new Language
                                           {
                                               Code = "zh-TW",
                                               Content = item["AwayCN"].ToString()
                                           },
                                       new Language
                                           {
                                               Code = "zh-CN",
                                               Content = item["AwayC_UNI"].ToString()
                                           }
                                   };
            return away;
        }

        /// <summary>
        /// Creates the home.
        /// </summary>
        /// <param name="item">The data row.</param>
        /// <returns>List of object Language.</returns>
        private static List<Language> CreateHome(DataRow item)
        {
            var home = new List<Language>
                                   {
                                       new Language
                                           {
                                               Code = "en-US",
                                               Content = item["Home"].ToString()
                                           },
                                       new Language
                                           {
                                               Code = "zh-TW",
                                               Content = item["HomeCN"].ToString()
                                           },
                                       new Language
                                           {
                                               Code = "zh-CN",
                                               Content = item["HomeC_UNI"].ToString()
                                           }
                                   };
            return home;
        }

        /// <summary>
        /// Creates the name of the bookie.
        /// </summary>
        /// <returns>List of object Language.</returns>
        private static List<Language> CreateBookieName()
        {
            var bookieName = new List<Language>
                                     {
                                         new Language
                                             {
                                                 Code = "en-US",
                                                 Content = string.Empty
                                             },
                                         new Language
                                             {
                                                 Code = "zh-TW",
                                                 Content = string.Empty
                                             },
                                         new Language
                                             {
                                                 Code = "zh-CN",
                                                 Content = string.Empty
                                             }
                                     };
            return bookieName;
        }

        /// <summary>
        /// Creates the league.
        /// </summary>
        /// <param name="item">The data row.</param>
        /// <returns>IList of object Language.</returns>
        private static IList<Language> CreateLeague(DataRow item)
        {
            IList<Language> content = new List<Language>
                                                  {
                                                      new Language
                                                          {
                                                              Code = "en-US",
                                                              Content = item["LeagueName"].ToString()
                                                          },
                                                      new Language
                                                          {
                                                              Code = "zh-TW",
                                                              Content = item["LeagueNameCN"].ToString()
                                                          },
                                                      new Language
                                                          {
                                                              Code = "zh-CN",
                                                              Content = item["LeagueNameC_UNI"].ToString()
                                                          }
                                                  };
            return content;
        }

        /// <summary>
        /// Creates the team.
        /// </summary>
        /// <param name="item">The data row.</param>
        /// <returns>List of object Language.</returns>
        private static List<Language> CreateTeam(DataRow item)
        {
            var team = new List<Language>
                               {
                                   new Language
                                       {
                                           Code = "en-US",
                                           Content = item["TeamName"].ToString()
                                       },
                                   new Language
                                       {
                                           Code = "zh-TW",
                                           Content = item["TeamNameCN"].ToString()
                                       },
                                   new Language
                                       {
                                           Code = "zh-CN",
                                           Content = item["TeamNameC_UNI"].ToString()
                                       }
                               };
            return team;
        }

        /// <summary>
        /// Continuously try a set with exponential back-off until number of tries or
        /// successful.  The exponential back-off will wait a maximum of 1 second, or whatever.
        /// </summary>
        /// <param name="key"> Key to set.</param>
        /// <param name="value"> Value to set.</param>
        /// <param name="tries"> Number of tries before giving up.</param>
        /// <returns>True if success or False.</returns>
        private static bool Set(string key, object value, int tries)
        {
            var backoffExp = 0;
            var tryAgain = false;
            IStoreOperationResult result = null;

            try
            {
                do
                {
                    if (backoffExp > tries)
                    {
                        throw new ApplicationException("Could not perform a set after " + tries + " tries.");
                    }

                    result = _Cache.ExecuteStore(StoreMode.Set, key, value);
                    if (result.Success)
                    {
                        break;
                    }

                    if (backoffExp > 0)
                    {
                        var backOffMillis = Math.Pow(2, backoffExp);
                        backOffMillis = Math.Min(1000, backOffMillis); ////1 sec max
                        Thread.Sleep((int)backOffMillis);
                        LogService.Error("Couchbase error! --- Backing off, tries so far: " + backoffExp);                        
                    }

                    backoffExp++;

                    if (!result.Success)
                    {
                        var message = result.InnerResult != null ? result.InnerResult.Message : result.Message;
                        LogService.Error("Couchbase error! --- Failed with status: " + message);                        
                    }

                    ////Future versions of the .NET client will flatten the results and make checking for InnerResult objects unnecessary
                    tryAgain = (result.Message != null && result.Message.Contains("Temporary failure")) ||
                                    (result.InnerResult != null && result.InnerResult.Message.Contains("Temporary failure"));
                }
                while (tryAgain);
            }
            catch (Exception ex)
            {
                LogService.Error("Couchbase error! --- Interrupted while trying to set.  Exception:" + ex.Message);                
            }
            //// note that other failure cases fall through.  status.isSuccess() can be
            //// checked for success or failure or the message can be retrieved.
            return result.Success;
        }

        #endregion
    }
}
