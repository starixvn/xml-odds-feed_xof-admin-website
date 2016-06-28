// --------------------------------------------------------------------------
// <copyright file="OddsFeed.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>
namespace Fanex.OddsFeed.Library
{
    using Matchs.OddNodeBuilder;
    using Fanex.OddsFeed.Library.Matchs;
    using Fanex.OddsFeed.Library.Outrights;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    /// <summary>
    /// Class OddsFeed.
    /// </summary>
    public class OddsFeed
    {
        private string _bookiename;
        private IList<Match> _liveMatches;
        private IList<Match> _deadballMatches;
        private IList<Event> _events;
        private OddsProfile _op;
        private string _timestamp;

        /// <summary>
        /// Initializes a new instance of the <see cref="OddsFeed"/> class.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <param name="system">The system.</param>
        /// <param name="domainName">Name of the domain.</param>
        /// <param name="ip">The login IP.</param>
        /// <param name="timestamp">The timestamp.</param>
        public OddsFeed(OddsProfile profile, string system, string domainName, string ip, string timestamp)
        {
            _bookiename = system;
            ////get profile
            //// op = new OddsProfile(DbConnect.GetProfile(profileId));
            _op = profile;
            this._timestamp = timestamp;

            var oddsdata = Caching.GetData(profile.DelayTimes);

            _liveMatches = oddsdata.LiveMatches;
            _deadballMatches = oddsdata.DeadballMatches;

            _events = oddsdata.Events;
        }

        /// <summary>
        /// Builds the XML document.
        /// </summary>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="isSample">If set to <c>true</c> [is sample].</param>
        /// <param name="isOutrights">If set to <c>true</c> [is outrights].</param>
        /// <returns>Xml Element.</returns>
        public XmlElement BuildXmlDocument(string timestamp, OddsFeedFilter filter, bool isSample = false, bool isOutrights = true)
        {
            var xd = new XmlDocument();
            var xmlDeclaration = xd.CreateXmlDeclaration("1.0", "utf-8", null);
            xd.InsertBefore(xmlDeclaration, xd.DocumentElement);

            var root = xd.CreateElement("oddsfeed");

            ////build outrights
            var outrights = BuildOutRight(root, xd, filter, isOutrights, isSample);
            if (outrights != null)
            {
                root.AppendChild(outrights);
            }

            ////build match
            var matchs = BuildMatch(root, xd, filter, isSample);
            if (matchs != null)
            {
                if ((isSample && !isOutrights) || !_op.OddsType.Contains(8))
                {
                    return (XmlElement)matchs;
                }

                root.AppendChild(matchs);
            }

            return root;
        }

        /// <summary>
        /// Builds the XML event.
        /// </summary>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="isSample">If set to <c>true</c> [is sample].</param>
        /// <param name="isOutrights">If set to <c>true</c> [is outrights].</param>
        /// <returns>Xml Element.</returns>
        public XmlElement BuildXmlEvent(string timestamp, OddsFeedFilter filter, bool isSample = false, bool isOutrights = true)
        {
            var xd = new XmlDocument();
            var xmlDeclaration = xd.CreateXmlDeclaration("1.0", "utf-8", null);
            xd.InsertBefore(xmlDeclaration, xd.DocumentElement);

            var root = xd.CreateElement("eventfeed");

            ////build outrights
            var outrights = BuildOutRightEvent(xd, filter, isOutrights, isSample);
            if (outrights != null)
            {
                root.AppendChild(outrights);
            }

            ////build match            
            var matchs = BuildMatchEvent(xd, filter, isSample);
            if (matchs != null)
            {
                if (isSample && !isOutrights)
                {
                    return (XmlElement)matchs;
                }

                root.AppendChild(matchs);
            }

            return root;
        }

        /// <summary>
        /// Queries the matches.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="changeTime">If set to <c>true</c> [change time].</param>
        /// <param name="ts">The change time.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>IList Match.</returns>
        private IList<Match> QueryMatches(IList<Match> source, bool changeTime, int ts, OddsFeedFilter filter)
        {
            IList<Match> filteredMatches = null;
            filteredMatches = (from i in source
                               where (changeTime ? i.TimeStamp > ts : true) &&
                                         ////filter by sports
                                         (_op.Sports == null ? true : (_op.Sports.Count != 0 ? _op.Sports.Contains(i.SportId) : false)) &&
                                         (filter.Sports == null || filter.Sports.Count == 0 ? true : filter.Sports.Contains(i.SportId)) &&
                                         ////filter by leagues
                                         (_op.Leagues == null ? true : (_op.Leagues.Count != 0 ? _op.Leagues.Contains(i.LeagueId) : false)) &&
                                         (filter.Leagues == null || filter.Leagues.Count == 0 ? true : FilterByLeague(filter.Leagues, i.League[0].Content.ToLower())) &&
                                         ////filter by countries
                                         (_op.Countries == null ? true : (_op.Countries.Count != 0 ? _op.Countries.Contains(i.CountryCode.ToLower()) : false)) &&
                                         (filter.Countries == null || filter.Countries.Count == 0 ? true : filter.Countries.Contains(i.CountryCode.ToLower())) &&
                                         ////filter by keyword
                                         (string.IsNullOrEmpty(_op.LeagueKeyword) ? true : i.League[0].Content.ToLower().Contains(_op.LeagueKeyword.ToLower()))
                               select i).ToList();
            return filteredMatches;
        }

        #region XmlDocument

        /// <summary>
        /// Builds the out right.
        /// </summary>
        /// <param name="root">The XmlElement.</param>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="isOutrights">If set to <c>true</c> [is outrights].</param>
        /// <param name="isSample">If set to <c>true</c> [is sample].</param>
        /// <returns>The Xml Node.</returns>
        private XmlNode BuildOutRight(XmlElement root, XmlDocument xd, OddsFeedFilter filter, bool isOutrights = true, bool isSample = false)
        {
            ////build outrights
            if (_op.OddsType.Contains(8) && _events != null)
            {
                var outrights = xd.CreateNode(XmlNodeType.Element, "outrights", null);
                var filteredOutrights = (from i in _events
                                         where
                                         ////filter by sports                                            
                                         (_op.Sports == null ? true : (_op.Sports.Count != 0 ? _op.Sports.Contains(i.SportId) : false)) &&
                                         (filter.Sports == null || filter.Sports.Count == 0 ? true : filter.Sports.Contains(i.SportId)) &&
                                         ////filter by outrights
                                         (_op.Outrights == null ? true : (_op.Outrights.Count != 0 ? _op.Outrights.Contains(i.Id) : false)) &&
                                         ////filter by leagues
                                         (filter.Leagues == null || filter.Leagues.Count == 0 ? true : FilterByLeague(filter.Leagues, i.Content[0].Content.ToLower())) &&
                                         ////filter by countries                                            
                                         (_op.Countries == null ? true : (_op.Countries.Count != 0 ? _op.Countries.Contains(i.CountryCode.ToLower()) : false)) &&
                                         (filter.Countries == null || filter.Countries.Count == 0 ? true : filter.Countries.Contains(i.CountryCode.ToLower())) &&
                                         ////filter by keyword
                                         (string.IsNullOrEmpty(_op.OutrightsKeyword) ? true : i.Content[0].Content.ToLower().Contains(_op.OutrightsKeyword.ToLower()))
                                         select i).ToList();

                if (isSample && isOutrights && filteredOutrights.Count > 0)
                {
                    outrights.AppendChild(filteredOutrights[0].BuildXmlNodeOutRight(xd, _op));
                }
                else if (!isSample)
                {
                    //// outright bettype = 10
                    if (filter.BetTypes == null || filter.BetTypes.Count == 0 || (filter.BetTypes != null && filter.BetTypes.Contains(10)))
                    {
                        foreach (var item in filteredOutrights)
                        {
                            outrights.AppendChild(item.BuildXmlNodeOutRight(xd, _op));
                        }
                    }
                }

                return outrights;
            }

            return null;
        }

        /// <summary>
        /// Builds the match.
        /// </summary>
        /// <param name="root">The XmlElement.</param>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="isSample">If set to <c>true</c> [is sample].</param>
        /// <returns>The Xml Node.</returns>
        private XmlNode BuildMatch(XmlElement root, XmlDocument xd, OddsFeedFilter filter, bool isSample = false)
        {
            var changetime = !string.IsNullOrEmpty(_timestamp); //// && (op.SpParams == "timestamp");
            int ts = changetime ? int.Parse(_timestamp) : 0;

            IList<Match> filteredDeadballMatches = null;
            IList<Match> filteredLiveMatches = null;
            ////filter by deadball
            if (_op.OddsType.Contains(2) && _deadballMatches != null)
            {
                filteredDeadballMatches = QueryMatches(_deadballMatches, changetime, ts, filter);
            }

            ////filter by Live
            if (_op.OddsType.Contains(4) && _liveMatches != null)
            {
                filteredLiveMatches = QueryMatches(_liveMatches, changetime, ts, filter);
            }

            if (filteredLiveMatches != null || filteredDeadballMatches != null)
            {
                var ms = xd.CreateNode(XmlNodeType.Element, "matches", null);

                var sampleMatch = false;
                BuildDeadBallMatch(ref ms, xd, filteredDeadballMatches, filter, isSample, ref sampleMatch);
                BuildLiveMatch(ref ms, xd, filteredLiveMatches, filter, isSample, sampleMatch);

                if (ms.ChildNodes.Count > 0)
                {
                    var lastUpdate = xd.CreateAttribute("lastupdate");
                    lastUpdate.Value = DateTime.Now.AddHours(12).ToString();
                    ms.Attributes.Append(lastUpdate);

                    BuildTimeStamp(ref ms, xd, filteredDeadballMatches, filteredLiveMatches);

                    BuildOddsType(ref ms, xd);
                }

                return ms;
            }

            return null;
        }

        /// <summary>
        /// Builds the time stamp.
        /// </summary>
        /// <param name="ms">The XmlNode.</param>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="filteredDeadballMatches">The filtered dead ball matches.</param>
        /// <param name="filteredLiveMatches">The filtered live matches.</param>
        private void BuildTimeStamp(ref XmlNode ms, XmlDocument xd, IList<Match> filteredDeadballMatches, IList<Match> filteredLiveMatches)
        {
            ////timestamp                
            ////if (!string.IsNullOrEmpty(timestamp))
            ////{
            ////    var mTimestamp = xd.CreateAttribute("timestamp");
            ////    mTimestamp.Value = timestamp;
            ////    ms.Attributes.Append(mTimestamp);
            ////}

            ////timestamp
            ////get maxTimeStamp of matches of bookies                
            int maxTimeStamp = 0;
            if (filteredLiveMatches != null)
            {
                foreach (var match in filteredLiveMatches)
                {
                    if (match.TimeStamp > maxTimeStamp)
                    {
                        maxTimeStamp = match.TimeStamp;
                    }
                }
            }

            if (filteredDeadballMatches != null)
            {
                foreach (var match in filteredDeadballMatches)
                {
                    if (match.TimeStamp > maxTimeStamp)
                    {
                        maxTimeStamp = match.TimeStamp;
                    }
                }
            }

            var tmpTimestamp = xd.CreateAttribute("timestamp");
            tmpTimestamp.Value = maxTimeStamp.ToString();
            ms.Attributes.Append(tmpTimestamp);
        }

        /// <summary>
        /// Builds the type of the odds.
        /// </summary>
        /// <param name="ms">The XmlNode.</param>
        /// <param name="xd">The XmlDocument.</param>
        private void BuildOddsType(ref XmlNode ms, XmlDocument xd)
        {
            ////oddstype
            var oddstype = xd.CreateAttribute("oddstype");
            switch (_op.OddRegion)
            {
                case 1:
                    oddstype.Value = "Malay";
                    break;
                case 2:
                    oddstype.Value = "HongKong";
                    break;
                case 3:
                    oddstype.Value = "Decimal";
                    break;
                case 4:
                    oddstype.Value = "Indo";
                    break;
                case 5:
                    oddstype.Value = "US";
                    break;
                default:
                    oddstype.Value = "Malay";
                    break;
            }

            ms.Attributes.Append(oddstype);
        }

        /// <summary>
        /// Builds the dead ball match.
        /// </summary>
        /// <param name="ms">The XmlNode.</param>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="filteredDeadballMatches">The filtered dead ball matches.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="isSample">If set to <c>true</c> [is sample].</param>
        /// <param name="sampleMatch">If set to <c>true</c> [sample match].</param>
        private void BuildDeadBallMatch(
            ref XmlNode ms,
            XmlDocument xd,
            IList<Match> filteredDeadballMatches,
            OddsFeedFilter filter,
            bool isSample,
            ref bool sampleMatch)
        {
            if (filteredDeadballMatches != null)
            {
                if (isSample && filteredDeadballMatches.Count > 0)
                {
                    var matchNode = filteredDeadballMatches[0].BuildXmlNodeMatch(xd, _op, _bookiename, filter, isSample);
                    if (matchNode != null)
                    {
                        ms.AppendChild(matchNode);
                    }

                    sampleMatch = true;
                }
                else
                {
                    foreach (var item in filteredDeadballMatches)
                    {
                        var matchNode = item.BuildXmlNodeMatch(xd, _op, _bookiename, filter);
                        if (matchNode != null)
                        {
                            ms.AppendChild(matchNode);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Builds the live match.
        /// </summary>
        /// <param name="ms">The XmlNode.</param>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="filteredLiveMatches">The filtered live matches.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="isSample">If set to <c>true</c> [is sample].</param>
        /// <param name="sampleMatch">If set to <c>true</c> [sample match].</param>
        private void BuildLiveMatch(ref XmlNode ms, XmlDocument xd, IList<Match> filteredLiveMatches, OddsFeedFilter filter, bool isSample, bool sampleMatch)
        {
            if (filteredLiveMatches != null)
            {
                if (isSample && filteredLiveMatches.Count > 0)
                {
                    if (!sampleMatch)
                    {
                        var matchNode = filteredLiveMatches[0].BuildXmlNodeMatch(xd, _op, _bookiename, filter, isSample);
                        if (matchNode != null)
                        {
                            ms.AppendChild(matchNode);
                        }
                    }
                }
                else
                {
                    foreach (var item in filteredLiveMatches)
                    {
                        var matchNode = item.BuildXmlNodeMatch(xd, _op, _bookiename, filter);
                        if (matchNode != null)
                        {
                            ms.AppendChild(matchNode);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check a league exists in list.
        /// </summary>
        /// <param name="searchLeagues">The list of leagues.</param>
        /// <param name="league">The league.</param>
        /// <returns>Return true if exists else return false.</returns>
        private bool FilterByLeague(IList<string> searchLeagues, string league)
        {
            foreach (string item in searchLeagues)
            {
                if (league.Contains(item))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region XmlEvent

        /// <summary>
        /// Builds the out right event.
        /// </summary>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="isOutrights">If set to <c>true</c> [is outrights].</param>
        /// <param name="isSample">If set to <c>true</c> [is sample].</param>
        /// <returns>The Xml Node.</returns>
        private XmlNode BuildOutRightEvent(XmlDocument xd, OddsFeedFilter filter, bool isOutrights, bool isSample)
        {
            var outrights = xd.CreateNode(XmlNodeType.Element, "outrights", null);

            if (_op.OddsType.Contains(8) && _events != null)
            {
                var filteredOutrights = (from i in _events
                                         where
                                         ////filter by sports                                            
                                         (_op.Sports == null ? true : (_op.Sports.Count != 0 ? _op.Sports.Contains(i.SportId) : false)) &&
                                         (filter.Sports == null || filter.Sports.Count == 0 ? true : filter.Sports.Contains(i.SportId)) &&
                                         ////filter by outrights
                                         (_op.Outrights == null ? true : (_op.Outrights.Count != 0 ? _op.Outrights.Contains(i.Id) : false)) &&
                                         ////filter by leagues
                                         (filter.Leagues == null || filter.Leagues.Count == 0 ? true : FilterByLeague(filter.Leagues, i.Content[0].Content.ToLower())) &&
                                         ////filter by countries                                            
                                         (_op.Countries == null ? true : (_op.Countries.Count != 0 ? _op.Countries.Contains(i.CountryCode.ToLower()) : false)) &&
                                         (filter.Countries == null || filter.Countries.Count == 0 ? true : filter.Countries.Contains(i.CountryCode.ToLower())) &&
                                         ////filter by keyword
                                         (string.IsNullOrEmpty(_op.OutrightsKeyword) ? true : i.Content[0].Content.ToLower().Contains(_op.OutrightsKeyword.ToLower()))
                                         select i).ToList();

                if (isSample && isOutrights && filteredOutrights.Count > 0)
                {
                    outrights.AppendChild(filteredOutrights[0].BuildXmlNodeOutRightEvent(xd, _op));
                }
                else if (!isSample)
                {
                    foreach (var item in filteredOutrights)
                    {
                        outrights.AppendChild(item.BuildXmlNodeOutRightEvent(xd, _op));
                    }
                }
            }

            return outrights;
        }

        /// <summary>
        /// Builds the match event.
        /// </summary>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="isSample">If set to <c>true</c> [is sample].</param>
        /// <returns>The Xml Node.</returns>
        private XmlNode BuildMatchEvent(XmlDocument xd, OddsFeedFilter filter, bool isSample)
        {
            var changetime = !string.IsNullOrEmpty(_timestamp); ////&& (op.SpParams == "timestamp");
            int ts = changetime ? int.Parse(_timestamp) : 0;

            IList<Match> filteredDeadballMatches = null;
            IList<Match> filteredLiveMatches = null;
            ////filter by deadball
            if (_op.OddsType.Contains(2) && _deadballMatches != null)
            {
                filteredDeadballMatches = QueryMatches(_deadballMatches, changetime, ts, filter);
            }

            ////filter by Live
            if (_op.OddsType.Contains(4) && _liveMatches != null)
            {
                filteredLiveMatches = QueryMatches(_liveMatches, changetime, ts, filter);
            }

            var fullMatches = FilterMatchEvent(filteredDeadballMatches, filteredLiveMatches, isSample);

            if (fullMatches.Count > 0)
            {
                var ms = xd.CreateNode(XmlNodeType.Element, "matches", null);

                if (isSample)
                {
                    ms.AppendChild(fullMatches[0].BuildXmlNodeEvent(xd, _op, _bookiename));
                }
                else
                {
                    BuildLeague(ms, xd, fullMatches);
                }

                return ms;
            }

            return null;
        }

        /// <summary>
        /// Filters the match event.
        /// </summary>
        /// <param name="filteredDeadballMatches">The filtered dead ball matches.</param>
        /// <param name="filteredLiveMatches">The filtered live matches.</param>
        /// <param name="isSample">If set to <c>true</c> [is sample].</param>
        /// <returns>List Match.</returns>
        private List<Match> FilterMatchEvent(IList<Match> filteredDeadballMatches, IList<Match> filteredLiveMatches, bool isSample)
        {
            var fullMatches = new List<Match>();
            if (filteredDeadballMatches != null)
            {
                fullMatches.AddRange(filteredDeadballMatches);
            }

            if (filteredLiveMatches != null)
            {
                if (isSample)
                {
                    if (fullMatches.Count > 0)
                    {
                        fullMatches.AddRange(filteredLiveMatches);
                    }
                }
                else
                {
                    fullMatches.AddRange(filteredLiveMatches);
                }
            }

            return fullMatches;
        }

        /// <summary>
        /// Builds the league.
        /// </summary>
        /// <param name="ms">The XmlNode.</param>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="fullMatches">The full matches.</param>
        private void BuildLeague(XmlNode ms, XmlDocument xd, List<Match> fullMatches)
        {
            var leagues = new List<League>();

            foreach (var match in fullMatches)
            {
                var lgs = leagues.Where(l => l.LeagueId == match.LeagueId).FirstOrDefault();
                if (lgs != null)
                {
                    lgs.Matches.Add(match);
                }
                else
                {
                    var league = new League();
                    league.LeagueId = match.LeagueId;
                    league.Matches = new List<Match>();
                    league.Matches.Add(match);
                    leagues.Add(league);
                }
            }

            BuildLeagueNode(ms, xd, leagues);
        }

        /// <summary>
        /// Builds the league node.
        /// </summary>
        /// <param name="ms">The XmlNode.</param>
        /// <param name="xd">The XmlDocument.</param>
        /// <param name="leagues">The leagues.</param>
        private void BuildLeagueNode(XmlNode ms, XmlDocument xd, List<League> leagues)
        {
            foreach (var lg in leagues)
            {
                var leagueNode = xd.CreateNode(XmlNodeType.Element, "league", null);
                var leagueAttId = xd.CreateAttribute("id");
                leagueAttId.Value = lg.LeagueId.ToString();
                leagueNode.Attributes.Append(leagueAttId);

                if (_op.Languages.Contains("en-US"))
                {
                    var ge = xd.CreateAttribute("ge");
                    ge.Value = BetOddNodeBuilder.EscapeXML(lg.Matches.First().League.Where(l => l.Code == "en-US").SingleOrDefault().Content);
                    leagueNode.Attributes.Append(ge);
                }

                if (_op.Languages.Contains("zh-TW"))
                {
                    var gs = xd.CreateAttribute("gs");
                    gs.Value = BetOddNodeBuilder.EscapeXML(lg.Matches.First().League.Where(l => l.Code == "zh-TW").SingleOrDefault().Content);
                    leagueNode.Attributes.Append(gs);
                }

                if (_op.Languages.Contains("zh-CN"))
                {
                    var gt = xd.CreateAttribute("gt");
                    gt.Value = BetOddNodeBuilder.EscapeXML(lg.Matches.First().League.Where(l => l.Code == "zh-CN").SingleOrDefault().Content);
                    leagueNode.Attributes.Append(gt);
                }

                var leagueAttSport = xd.CreateAttribute("sport");
                leagueAttSport.Value = lg.Matches.First().Sport;
                leagueNode.Attributes.Append(leagueAttSport);

                foreach (var item in lg.Matches)
                {
                    leagueNode.AppendChild(item.BuildXmlNodeEvent(xd, _op, _bookiename));
                }

                ms.AppendChild(leagueNode);
            }
        }

        #endregion
    }
}
