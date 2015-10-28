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

            var liveFlexOdds = liveodds.ResultSet.Tables[1].AsEnumerable().Select(m => new Model.OddFlex() { }).ToList();
            var deadballFlexOdds = deadballOdds.ResultSet.Tables[1].AsEnumerable().Select(m => new Model.OddFlex() { }).ToList();

            ////live
            BuildMatchOdds(liveodds.ResultSet.Tables[0], liveFlexOdds, true, liveMatches);
            ////deadball
            BuildMatchOdds(deadballOdds.ResultSet.Tables[0], deadballFlexOdds, false, deadballMatches);
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
        private static void BuildMatchOdds(DataTable dataTable, List<Model.OddFlex> flexOdds, bool isLive, IList<Match> matches)
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
                var odd = CreateMatchOdd(item, flexOdds);

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
        private static MatchOdd CreateMatchOdd(DataRow item, List<Model.OddFlex> flexOdds)
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
                case 13:
                    ////Clean Sheet
                    return CreateMatchOddBettype13(item);
                case 14:
                    ////first-last
                    return CreateMatchOddBettype14(item);
                case 15:
                    return CreateMatchOddBettype15(item);
                case 16:
                    ////HT/FT
                    return CreateMatchOddBettype16(item);
                case 17:
                    return CreateMatchOddBettype17(item);
                case 18:
                    return CreateMatchOddBettype18(item);
                case 20:
                    ////HT/FT
                    return CreateMatchOddBettype20(item);
                case 21:
                    return CreateMatchOddBettype21(item);

                case 30:
                    return CreateMatchOddBettype30(item);
                case 121:
                    return CreateMatchOddBettype121(item);
                case 122:
                    return CreateMatchOddBettype122(item);
                case 123:
                    return CreateMatchOddBettype123(item);
                case 126:
                    return CreateMatchOddBettype126(item);
                case 127:
                    return CreateMatchOddBettype127(item);
                case 128:
                    return CreateMatchOddBettype128(item);
                case 133:
                    return CreateMatchOddBettype133(item);
                case 134:
                    return CreateMatchOddBettype134(item);
                case 135:
                    return CreateMatchOddBettype135(item);
                case 140:
                    return CreateMatchOddBettype140(item);
                case 141:
                    return CreateMatchOddBettype141(item);
                case 142:
                    return CreateMatchOddBettype142(item);

                case 145:
                    return CreateMatchOddBettype145(item);
                case 146:
                    return CreateMatchOddBettype146(item);
                case 147:
                    return CreateMatchOddBettype147(item);
                case 148:
                    return CreateMatchOddBettype148(item);
                case 149:
                    return CreateMatchOddBettype149(item);
                case 150:
                    return CreateMatchOddBettype150(item);
                case 151:
                    return CreateMatchOddBettype151(item);
                case 152:
                    return CreateMatchOddBettype152(item, flexOdds);
                case 153:
                    return CreateMatchOddBettype153(item);
                case 154:
                    return CreateMatchOddBettype154(item);
                case 155:
                    return CreateMatchOddBettype155(item);
                case 156:
                    return CreateMatchOddBettype156(item);
                case 157:
                    return CreateMatchOddBettype157(item);
                case 159:
                    return CreateMatchOddBettype159(item);
                case 160:
                    return CreateMatchOddBettype160(item);
                case 161:
                    return CreateMatchOddBettype161(item);
                case 162:
                    return CreateMatchOddBettype162(item);
                case 163:
                    return CreateMatchOddBettype163(item);
                case 164:
                    return CreateMatchOddBettype164(item);
                case 165:
                    return CreateMatchOddBettype165(item);
                case 166:
                    return CreateMatchOddBettype166(item);
                case 167:
                    return CreateMatchOddBettype167(item);
                case 168:
                    return CreateMatchOddBettype168(item);
                case 169:
                    return CreateMatchOddBettype169(item);
                case 170:
                    return CreateMatchOddBettype170(item);
                case 171:
                    return CreateMatchOddBettype171(item);
                case 172:
                    return CreateMatchOddBettype172(item);
                case 173:
                    return CreateMatchOddBettype173(item);
                case 174:
                    return CreateMatchOddBettype174(item);
                case 175:
                    return CreateMatchOddBettype175(item);
                case 176:
                    return CreateMatchOddBettype176(item);
                case 177:
                    return CreateMatchOddBettype177(item);
                case 178:
                    return CreateMatchOddBettype178(item);
                case 179:
                    return CreateMatchOddBettype179(item);
                case 180:
                    return CreateMatchOddBettype180(item);
                case 181:
                    return CreateMatchOddBettype181(item);
                case 182:
                    return CreateMatchOddBettype182(item);
                case 183:
                    return CreateMatchOddBettype183(item);
                case 184:
                    return CreateMatchOddBettype184(item);
                case 185:
                    return CreateMatchOddBettype185(item);
                case 186:
                    return CreateMatchOddBettype186(item);
                case 187:
                    return CreateMatchOddBettype187(item);
                case 188:
                    return CreateMatchOddBettype188(item);
                case 189:
                    return CreateMatchOddBettype189(item);
                case 190:
                    return CreateMatchOddBettype190(item);
                case 191:
                    return CreateMatchOddBettype191(item);
                case 192:
                    return CreateMatchOddBettype192(item);
                case 193:
                    return CreateMatchOddBettype193(item);
                case 194:
                    return CreateMatchOddBettype194(item);
                case 195:
                    return CreateMatchOddBettype195(item);
                case 196:
                    return CreateMatchOddBettype196(item);
                case 197:
                    return CreateMatchOddBettype197(item);
                case 198:
                    return CreateMatchOddBettype198(item);
                case 199:
                    return CreateMatchOddBettype199(item);
                case 200:
                    return CreateMatchOddBettype200(item);
                case 201:
                    return CreateMatchOddBettype201(item);
                case 202:
                    return CreateMatchOddBettype202(item);
                case 203:
                    return CreateMatchOddBettype203(item);
                case 204:
                    return CreateMatchOddBettype204(item);
                case 205:
                    return CreateMatchOddBettype205(item);
                case 206:
                    return CreateMatchOddBettype206(item);
                case 207:
                    return CreateMatchOddBettype207(item);
                case 208:
                    return CreateMatchOddBettype208(item);
                case 209:
                    return CreateMatchOddBettype209(item);
                case 210:
                    return CreateMatchOddBettype210(item);
                case 211:
                    return CreateMatchOddBettype211(item);
                case 212:
                    return CreateMatchOddBettype212(item);
                case 213:
                    return CreateMatchOddBettype213(item);
                case 214:
                    return CreateMatchOddBettype214(item);
                case 215:
                    return CreateMatchOddBettype215(item);
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

        // Generated by tool
        private static MatchOdd CreateMatchOddBettype21(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "ml_1",
                O1 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["Odds1A"]))),
                O2 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["Odds2A"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype13(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "so",
                HY = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs11"]))),
                HN = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs10"]))),
                AY = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs21"]))),
                AN = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs20"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype30(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "cs_1",
                C10 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs10"]))),
                C20 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs20"]))),
                C21 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs21"]))),
                C30 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs30"]))),
                C31 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs31"]))),
                C32 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs32"]))),
                C40 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs40"]))),
                C01 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs01"]))),
                C02 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs02"]))),
                C12 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs12"]))),
                C03 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs03"]))),
                C13 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs13"]))),
                C23 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs23"]))),
                C04 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs04"]))),
                C00 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs00"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype121(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "hnb",
                A = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds2a"]))),
                D = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds1a"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype122(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "anb",
                H = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds2a"]))),
                D = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds1a"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype123(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "dnd",
                D = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds2a"]))),
                N = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds1a"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype126(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "tg_1",
                O1 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs00"]))),
                O2 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs01"]))),
                O3 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs10"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype127(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "fglg_1",
                HF = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs11"]))),
                HL = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs12"]))),
                AF = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs21"]))),
                AL = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs22"]))),
                NG = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs00"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype128(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "oe_12",
                OE = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs12"]))),
                EO = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs21"]))),
                OO = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs11"]))),
                EE = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs22"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype133(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "h2h",
                Y = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["com1"]))),
                N = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["com2"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype134(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "a2h",
                Y = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["com1"]))),
                N = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["com2"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype135(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "pen",
                Y = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["com1"]))),
                N = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["com2"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype140(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "high",
                FirstH = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs10"]))),
                SecondH = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs20"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype141(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "hhigh",
                FirstH = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs10"]))),
                SecondH = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs20"]))),
                T = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs00"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype142(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "ahigh",
                FirstH = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs10"]))),
                SecondH = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs20"]))),
                T = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs00"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype145(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "bs",
                Y = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["com1"]))),
                N = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["com2"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype146(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "bs_2",
                Y = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["com1"]))),
                N = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["com2"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype147(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "hs",
                Y = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["com1"]))),
                N = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["com2"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype148(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "as",
                Y = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["com1"]))),
                N = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["com2"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype149(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "hw",
                Y = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["com1"]))),
                N = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["com2"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype150(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "aw",
                Y = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["com1"]))),
                N = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["com2"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype151(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "x2_1",
                HD = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs10"]))),
                HA = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs00"]))),
                AD = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs20"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype153(DataRow item)
        {
            var h = Converter.ToDecimal(item["hdp2"]) - Converter.ToDecimal(item["hdp1"]);
            var o = h > 0 ? Converter.ToDecimal(item["odds2a"]) : Converter.ToDecimal(item["odds1a"]);
            var u = h > 0 ? Converter.ToDecimal(item["odds1a"]) : Converter.ToDecimal(item["odds2a"]);
            var odd = new MatchOdd
            {
                Type = "tah",
                O = string.Format("{0:0.#########}", o),
                U = string.Format("{0:0.#########}", u),
                H = string.Format("{0:0.#########}", h),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype154(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "sw",
                H = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds1a"]))),
                A = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds2a"]))),
                X = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["resourceID"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype155(DataRow item)
        {
            var h = Converter.ToDecimal(item["hdp2"]) - Converter.ToDecimal(item["hdp1"]);
            var o = h > 0 ? Converter.ToDecimal(item["odds2a"]) : Converter.ToDecimal(item["odds1a"]);
            var u = h > 0 ? Converter.ToDecimal(item["odds1a"]) : Converter.ToDecimal(item["odds2a"]);

            var odd = new MatchOdd
            {
                Type = "gh",
                O = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(o)),
                U = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(u)),
                H = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(h)),
                X = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["resourceID"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype156(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "gou",
                O = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds1a"]))),
                U = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds2a"]))),
                H = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["hdp1"]))),
                X = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["resourceID"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype157(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "oe",
                O = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds1a"]))),
                E = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds2a"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype159(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "etg",
                O01 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs01"]))),
                O02 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs02"]))),
                O03 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs03"]))),
                O04 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs04"]))),
                O05 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs10"]))),
                O06 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs20"]))),
                O07 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs30"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype160(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "ng",
                H = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["com1"]))),
                N = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["comx"]))),
                A = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["com2"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype161(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "heg",
                O1 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs00"]))),
                O2 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs01"]))),
                O3 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs02"]))),
                O4 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs03"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype162(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "aeg",
                O1 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs00"]))),
                O2 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs01"]))),
                O3 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs02"]))),
                O4 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs03"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype163(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "sttg",
                HO = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs01"]))),
                HU = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs00"]))),
                AO = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs05"]))),
                AU = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs04"]))),
                DO = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs03"]))),
                DU = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs02"]))),
                H = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["hdp1"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype164(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "nxtg_ex",
                H = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["com1"]))),
                N = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["comx"]))),
                A = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["com2"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype165(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "cs_ex1",
                CS00 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs00"]))),
                CS10 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs01"]))),
                CS01 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs03"]))),
                CS11 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs04"]))),
                CS20 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs02"]))),
                CS02 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs05"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype166(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "cs_ex",
                CS00 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs21"]))),
                CS10 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs12"]))),
                CS01 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs30"]))),
                CS11 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs32"]))),
                CS20 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs20"]))),
                CS02 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs13"]))),
                CS21 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs31"]))),
                CS12 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs32"]))),
                CS30 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs02"]))),
                CS03 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs23"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype167(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "st_ex1",
                H = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["com1"]))),
                N = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["comx"]))),
                A = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["com2"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype168(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "nxtrnd",
                H = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["com1"]))),
                A = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["com2"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype169(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "nxtg",
                O1 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs01"]))),
                O2 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs02"]))),
                O3 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs03"]))),
                O4 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs04"]))),
                O5 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs10"]))),
                O6 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs20"]))),
                O7 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs30"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype170(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "ts",
                H = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs00"]))),
                A = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs01"]))),
                B = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs02"]))),
                N = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs03"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype171(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "win",
                H1 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs01"]))),
                H2 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs02"]))),
                H3 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs03"]))),
                D = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs04"]))),
                A1 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs10"]))),
                A2 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs20"]))),
                A3 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs30"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype172(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "stts",
                HH = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs01"]))),
                DH = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs02"]))),
                AH = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs03"]))),
                HA = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs04"]))),
                DA = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs10"]))),
                AA = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs20"]))),
                N = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs30"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype173(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "ex",
                Y = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds1a"]))),
                N = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds2a"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype174(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "exg",
                Y = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds1a"]))),
                N = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds2a"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype175(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "mtd",
                H1 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs00"]))),
                H2 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs01"]))),
                H3 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs02"]))),
                A1 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs03"]))),
                A2 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs04"]))),
                A3 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs05"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype176(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "st_10",
                H = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["com1"]))),
                N = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["comx"]))),
                A = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["com2"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype177(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "st_2",
                H = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["com1"]))),
                N = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["comx"]))),
                A = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["com2"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype178(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "ou_2",
                O = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds1a"]))),
                U = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds2a"]))),
                H = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["hdp1"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype179(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "egs_1",
                O1 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs00"]))),
                O2 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs11"]))),
                O3 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs12"]))),
                O4 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs21"]))),
                O5 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs22"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype180(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "ng_1",
                H = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["com1"]))),
                N = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["comx"]))),
                A = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["com2"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype181(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "heg_1",
                O1 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs00"]))),
                O2 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs01"]))),
                O3 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs02"]))),
                O4 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs03"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype182(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "aeg_1",
                O1 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs00"]))),
                O2 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs01"]))),
                O3 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs02"]))),
                O4 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs03"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype183(DataRow item)
        {
            var h = Converter.ToDecimal(item["hdp2"]) - Converter.ToDecimal(item["hdp1"]);
            var o = h > 0 ? Converter.ToDecimal(item["odds2a"]) : Converter.ToDecimal(item["odds1a"]);
            var u = h > 0 ? Converter.ToDecimal(item["odds1a"]) : Converter.ToDecimal(item["odds2a"]);

            var odd = new MatchOdd
            {
                Type = "hdp_2",
                O = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(o)),
                U = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(u)),
                H = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(h)),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype184(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "oe_2",
                O = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds1a"]))),
                E = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds2a"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype185(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "dnb_2",
                H = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds1a"]))),
                A = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds2a"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype186(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "x2_2",
                HD = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs10"]))),
                HA = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs20"]))),
                AD = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs00"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype187(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "eg_2",
                O1 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs10"]))),
                O2 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs20"]))),
                O3 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs00"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype188(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "b_1",
                Y = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds1a"]))),
                N = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds2a"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype189(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "oyn",
                Y = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds1a"]))),
                N = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds2a"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype190(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "uyn",
                Y = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds1a"]))),
                N = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds2a"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype191(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "dnb_1",
                H = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds1a"]))),
                A = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds2a"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype192(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "fg10",
                O1 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs21"]))),
                O2 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs12"]))),
                O3 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs20"]))),
                O4 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs02"]))),
                O5 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs30"]))),
                O6 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs03"]))),
                O7 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs31"]))),
                O8 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs13"]))),
                O9 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs32"]))),
                O10 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs23"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype193(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "fg15",
                O1 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs01"]))),
                O2 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs02"]))),
                O3 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs03"]))),
                O4 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs04"]))),
                O5 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs10"]))),
                O6 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs20"]))),
                O7 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs30"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype194(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "coe",
                O = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds1a"]))),
                E = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds2a"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype195(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "hc",
                O1 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs00"]))),
                O2 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs01"]))),
                O3 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs02"]))),
                O4 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs03"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype196(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "ac",
                O1 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs00"]))),
                O2 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs01"]))),
                O3 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs02"]))),
                O4 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs03"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype197(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "hcou",
                O = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds1a"]))),
                U = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds2a"]))),
                H = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["hdp1"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype198(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "acou",
                O = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds1a"]))),
                U = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds2a"]))),
                H = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["hdp1"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype199(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "tc",
                O1 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs10"]))),
                O2 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs20"]))),
                O3 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs00"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype200(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "ec",
                O1 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs00"]))),
                O2 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs01"]))),
                O3 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs02"]))),
                O4 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs03"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype201(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "ec_1",
                O1 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs00"]))),
                O2 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs01"]))),
                O3 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs02"]))),
                O4 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs03"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype202(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "tc_1",
                O1 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs10"]))),
                O2 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs20"]))),
                O3 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs00"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype203(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "coe_1",
                O = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds1a"]))),
                E = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds2a"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype204(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "hcou_1",
                O = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds1a"]))),
                U = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds2a"]))),
                H = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["hdp1"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype205(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "acou_1",
                O = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds1a"]))),
                U = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds2a"]))),
                H = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["hdp1"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype206(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "fc",
                H = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs10"]))),
                N = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs00"]))),
                A = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs20"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype207(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "fc_1",
                H = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs10"]))),
                N = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs00"]))),
                A = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs20"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype208(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "lc",
                H = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs10"]))),
                N = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs00"]))),
                A = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs20"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype209(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "lc_1",
                H = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs10"]))),
                N = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs00"]))),
                A = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["cs20"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype210(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "sent",
                Y = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds1a"]))),
                N = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds2a"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype211(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "sent_1",
                Y = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds1a"]))),
                N = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds2a"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype212(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "hsent",
                Y = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds1a"]))),
                N = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds2a"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype213(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "hsent_1",
                Y = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds1a"]))),
                N = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds2a"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype214(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "asent",
                Y = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds1a"]))),
                N = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds2a"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype215(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "asent_1",
                Y = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds1a"]))),
                N = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds2a"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype17(DataRow item)
        {
            var h = Convert.ToDecimal(item["hdp2"]) - Convert.ToDecimal(item["hdp2"]);
            var o = h > 0 ? Convert.ToDecimal(item["odds2a"]) : Convert.ToDecimal(item["odds1a"]);
            var u = h > 0 ? Convert.ToDecimal(item["odds1a"]) : Convert.ToDecimal(item["odds2a"]);
            var odd = new MatchOdd
            {
                Type = "ah_2",
                O = string.Format("{0:0.#########}", o),
                U = string.Format("{0:0.#########}", u),
                H = string.Format("{0:0.#########}", h),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype18(DataRow item)
        {
            var odd = new MatchOdd
            {
                Type = "ou_2",
                O = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds1a"]))),
                U = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["odds2a"]))),
                H = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(item["hdp1"]))),
                Time = DateTime.Parse(item["ChangeTime"].ToString())
            };
            return odd;
        }

        private static MatchOdd CreateMatchOddBettype152(DataRow item, List<Model.OddFlex> flexOdds)
        {
            var odds = flexOdds.Where(i => i.OddId == Convert.ToInt32(item["oddsid"]));

            var odd = new MatchOdd
            {
                Type = "cs_12",
                O01 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "0:0/0:0")))),
                O02 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "0:0/1:0")))),
                O03 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "0:0/0:1")))),
                O04 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "0:0/2:0")))),
                O05 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "0:0/1:1")))),
                O06 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "0:0/0:2")))),
                O07 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "0:0/3:0")))),
                O08 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "0:0/2:1")))),
                O09 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "0:0/1:2")))),
                O10 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "0:0/0:3")))),
                O11 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "0:0/4+")))),
                O12 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "1:0/1:0")))),
                O13 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "1:0/2:0")))),
                O14 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "1:0/1:1")))),
                O15 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "1:0/3:0")))),
                O16 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "1:0/2:1")))),
                O17 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "1:0/1:2")))),
                O18 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "1:0/4+")))),
                O19 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "0:1/0:1")))),
                O20 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "0:1/1:1")))),
                O21 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "0:1/0:2")))),
                O22 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "0:1/2:1")))),
                O23 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "0:1/1:2")))),
                O24 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "0:1/0:3")))),
                O25 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "0:1/4+")))),
                O26 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "1:1/1:1")))),
                O27 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "1:1/2:1")))),
                O28 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "1:1/1:2")))),
                O29 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "1:1/4+")))),
                O30 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "2:0/2:0")))),
                O31 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "2:0/3:0")))),
                O32 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "2:0/3:1")))),
                O33 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "2:0/4+")))),
                O34 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "0:2/0:2")))),
                O35 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "0:2/1:2")))),
                O36 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "0:2/0:3")))),
                O37 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "0:2/4+")))),
                O38 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "2:1/2:1")))),
                O39 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "2:1/4+")))),
                O40 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "1:2/1:2")))),
                O41 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "1:2/4+")))),
                O42 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "3:0/3:0")))),
                O43 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "3:0/4+")))),
                O44 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "0:3/0:3")))),
                O45 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "0:3/4+")))),
                O46 = string.Format("{0:0.#########}", Utilities.ToDecimalOddsFromMalayOdds(Converter.ToDecimal(odds.Where(i => i.BetChoice == "4+/4+")))),
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
