namespace Fanex.OddsFeed.Library
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using Fanex.OddsFeed.Library.Matchs;
    using Fanex.OddsFeed.Library.Outrights;
    using Matchs.OddNodeBuilder;

    /// <summary>
    /// Class OddsFeed.
    /// </summary>
    public class OddsFeed
    {
        private string _bookieName;
        private string _timeStamp;
        private IList<Match> _liveMatches;
        private IList<Match> _deadballMatches;
        private IList<Event> _events;
        private OddsProfile _oddsProfile;

        public OddsFeed(OddsProfile profile, string system, string domainName, string ip, string timestamp)
        {
            _bookieName = system;
            _oddsProfile = profile;
            _timeStamp = timestamp;

            var oddsdata = Caching.GetData(profile.DelayTimes);

            _liveMatches = oddsdata.LiveMatches;
            _deadballMatches = oddsdata.DeadballMatches;

            _events = oddsdata.Events;
        }

        public XmlElement BuildXmlDocument(string timestamp, OddsFeedFilter filter, bool isSample = false, bool isOutrights = true)
        {
            var xmlDocument = CreateXmlDocument();
            var nodeRoot = xmlDocument.CreateElement("oddsfeed");

            if (IsShownOutright(isSample, isOutrights))
            {
                AddOutRight(nodeRoot, xmlDocument, filter, isSample, isOutrights);
            }

            AddMatch(nodeRoot, xmlDocument, filter, isSample, isOutrights);

            return nodeRoot;
        }

        private XmlDocument CreateXmlDocument()
        {
            var xmlDocument = new XmlDocument();
            var xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);

            xmlDocument.InsertBefore(xmlDeclaration, xmlDocument.DocumentElement);

            return xmlDocument;
        }

        private void AddOutRight(XmlElement nodeRoot, XmlDocument xmlDocument, OddsFeedFilter filter, bool isSample, bool isOutrights)
        {
            var outrights = BuildOutRight(nodeRoot, xmlDocument, filter, isOutrights, isSample);

            if (outrights != null)
            {
                nodeRoot.AppendChild(outrights);
            }
        }

        private void AddMatch(XmlElement nodeRoot, XmlDocument xmlDocument, OddsFeedFilter filter, bool isSample, bool isOutrights)
        {
            var matches = BuildMatch(nodeRoot, xmlDocument, filter, isSample);

            if (matches != null)
            {
                nodeRoot.AppendChild(matches);
            }
        }

        private bool IsShownOutright(bool isSample, bool isOutrights)
        {
            return _oddsProfile.OddsType.Contains(8) && (!isSample || isOutrights);
        }

        public XmlElement BuildXmlEvent(string timestamp, OddsFeedFilter filter, bool isSample = false, bool isOutrights = true)
        {
            var xmlDocument = CreateXmlDocument();
            var nodeRoot = xmlDocument.CreateElement("eventfeed");

            if (IsShownOutRightEvent(isSample, isOutrights))
            {
                AddOutRightEvent(nodeRoot, xmlDocument, filter, isOutrights, isSample);
            }

            AddMatchEvent(nodeRoot, xmlDocument, filter, isOutrights, isSample);

            return nodeRoot;
        }

        private void AddOutRightEvent(XmlElement nodeRoot, XmlDocument xmlDocument, OddsFeedFilter filter, bool isSample, bool isOutrights)
        {
            var outrights = BuildOutRightEvent(xmlDocument, filter, isOutrights, isSample);

            if (outrights != null)
            {
                nodeRoot.AppendChild(outrights);
            }
        }

        private void AddMatchEvent(XmlElement nodeRoot, XmlDocument xmlDocument, OddsFeedFilter filter, bool isSample, bool isOutrights)
        {
            var matches = BuildMatchEvent(xmlDocument, filter, isSample);

            if (matches != null)
            {
                nodeRoot.AppendChild(matches);
            }
        }

        private bool IsShownOutRightEvent(bool isSample, bool isOutrights)
        {
            return !isSample || isOutrights;
        }

        private IList<Match> QueryMatches(IList<Match> matches, bool changeTime, int timeStamp, OddsFeedFilter filter)
        {
            return (from match in matches
                    where (changeTime ? match.TimeStamp > timeStamp : true) &&
                                FilterSports(match, filter) &&
                                FilterLeagues(match, filter) &&
                                FilterCountries(match, filter) &&
                                FilterKeywords(match)
                    select match).ToList();
        }

        #region XmlDocument

        private XmlNode BuildOutRight(XmlElement root, XmlDocument xd, OddsFeedFilter filter, bool isOutrights = true, bool isSample = false)
        {
            ////build outrights
            if (_oddsProfile.OddsType.Contains(8) && _events != null)
            {
                var outrights = xd.CreateNode(XmlNodeType.Element, "outrights", null);
                var filteredOutrights = (from i in _events
                                         where
                                         ////filter by sports                                            
                                         (_oddsProfile.Sports == null ? true : (_oddsProfile.Sports.Count != 0 ? _oddsProfile.Sports.Contains(i.SportId) : false)) &&
                                         (filter.Sports == null || filter.Sports.Count == 0 ? true : filter.Sports.Contains(i.SportId)) &&
                                         ////filter by outrights
                                         (_oddsProfile.Outrights == null ? true : (_oddsProfile.Outrights.Count != 0 ? _oddsProfile.Outrights.Contains(i.Id) : false)) &&
                                         ////filter by leagues
                                         (filter.Leagues == null || filter.Leagues.Count == 0 ? true : FilterByLeague(filter.Leagues, i.Content[0].Content.ToLower())) &&
                                         ////filter by countries                                            
                                         (_oddsProfile.Countries == null ? true : (_oddsProfile.Countries.Count != 0 ? _oddsProfile.Countries.Contains(i.CountryCode.ToLower()) : false)) &&
                                         (filter.Countries == null || filter.Countries.Count == 0 ? true : filter.Countries.Contains(i.CountryCode.ToLower())) &&
                                         ////filter by keyword
                                         (string.IsNullOrEmpty(_oddsProfile.OutrightsKeyword) ? true : i.Content[0].Content.ToLower().Contains(_oddsProfile.OutrightsKeyword.ToLower()))
                                         select i).ToList();

                if (isSample && isOutrights && filteredOutrights.Count > 0)
                {
                    outrights.AppendChild(filteredOutrights[0].BuildXmlNodeOutRight(xd, _oddsProfile));
                }
                else if (!isSample)
                {
                    //// outright bettype = 10
                    if (filter.BetTypes == null || filter.BetTypes.Count == 0 || (filter.BetTypes != null && filter.BetTypes.Contains(10)))
                    {
                        foreach (var item in filteredOutrights)
                        {
                            outrights.AppendChild(item.BuildXmlNodeOutRight(xd, _oddsProfile));
                        }
                    }
                }

                return outrights;
            }

            return null;
        }

        private XmlNode BuildMatch(XmlElement nodeRoot, XmlDocument xmlDocument, OddsFeedFilter filter, bool isSample = false)
        {
            var changetime = !string.IsNullOrEmpty(_timeStamp);
            int timeStamp = changetime ? int.Parse(_timeStamp) : 0;

            var deadballMatches = GetDeadMatches(_deadballMatches, changetime, timeStamp, filter);
            var liveMatches = GetLiveMatches(_liveMatches, changetime, timeStamp, filter);

            if (liveMatches != null || deadballMatches != null)
            {
                var matchNode = xmlDocument.CreateNode(XmlNodeType.Element, "matches", null);

                var sampleMatch = false;
                BuildDeadBallMatchNode(matchNode, xmlDocument, deadballMatches, filter, isSample);
                BuildLiveMatchNode(matchNode, xmlDocument, liveMatches, filter, isSample, sampleMatch);

                if (matchNode.ChildNodes.Count > 0)
                {
                    var lastUpdate = xmlDocument.CreateAttribute("lastupdate");
                    lastUpdate.Value = DateTime.Now.AddHours(12).ToString();
                    matchNode.Attributes.Append(lastUpdate);

                    BuildTimeStamp(matchNode, xmlDocument, deadballMatches, liveMatches);
                    BuildOddsType(matchNode, xmlDocument);
                }

                return matchNode;
            }

            return null;
        }

        private IList<Match> GetDeadMatches(IList<Match> deadMatches, bool changeTime, int timeStamp, OddsFeedFilter filter)
        {
            return HaveDeadBallMatches()
                ? QueryMatches(deadMatches, changeTime, timeStamp, filter)
                : new List<Match>();
        }

        private bool HaveDeadBallMatches()
        {
            return _oddsProfile.OddsType.Contains(2) && _deadballMatches != null;
        }

        private IList<Match> GetLiveMatches(IList<Match> liveMatches, bool changeTime, int timeStamp, OddsFeedFilter filter)
        {
            return HaveLiveBallMatches()
                ? QueryMatches(liveMatches, changeTime, timeStamp, filter)
                : new List<Match>();
        }

        private bool HaveLiveBallMatches()
        {
            return _oddsProfile.OddsType.Contains(4) && _liveMatches != null;
        }

        private void BuildTimeStamp(
            XmlNode matchNode, 
            XmlDocument xmlDocument, 
            IList<Match> deadBallMatches, 
            IList<Match> liveMatches)
        {                     
            int deadBallTimeStamp = 0;
            int liveTimeStamp = 0;

            if (deadBallMatches.Any())
            {
                deadBallTimeStamp = deadBallMatches.Max(match => match.TimeStamp);
            }

            if (liveMatches.Any())
            {
                liveTimeStamp = liveMatches.Max(match => match.TimeStamp);
            }

            var maxTimeStamp = deadBallTimeStamp > liveTimeStamp ? deadBallTimeStamp : liveTimeStamp;
            var timeStampAttribute = xmlDocument.CreateAttribute("timestamp");

            timeStampAttribute.Value = maxTimeStamp.ToString();

            matchNode.Attributes.Append(timeStampAttribute);
        }

        private void BuildOddsType(XmlNode matchNode, XmlDocument xmlDocument)
        {
            var oddstype = xmlDocument.CreateAttribute("oddstype");

            switch (_oddsProfile.OddRegion)
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

            matchNode.Attributes.Append(oddstype);
        }

        private void BuildDeadBallMatchNode(
            XmlNode deadBallMatchNode,
            XmlDocument xmlDocument,
            IList<Match> deadBallMatches,
            OddsFeedFilter filter,
            bool isSample)
        {
            if (!deadBallMatches.Any())
            {
                return;
            }

            if (isSample)
            {
                AddSampleMatch(deadBallMatchNode, xmlDocument, deadBallMatches, filter, isSample);
            }
            else
            {
                AddAllMatches(deadBallMatchNode, xmlDocument, deadBallMatches, filter);
            }
        }

        private void BuildLiveMatchNode(
            XmlNode liveMatchNode,
            XmlDocument xmlDocument,
            IList<Match> liveMatches,
            OddsFeedFilter filter,
            bool isSample,
            bool sampleMatch)
        {
            if (!liveMatches.Any())
            {
                return;
            }

            if (isSample)
            {
                if (!sampleMatch)
                {
                    AddSampleMatch(liveMatchNode, xmlDocument, liveMatches, filter, isSample);
                }
            }
            else
            {
                AddAllMatches(liveMatchNode, xmlDocument, liveMatches, filter);
            }
        }

        private void AddSampleMatch(
            XmlNode matchNode,
            XmlDocument xmlDocument,
            IList<Match> matches,
            OddsFeedFilter filter,
            bool isSample)
        {
            var node = matches[0].BuildXmlNodeMatch(xmlDocument, _oddsProfile, _bookieName, filter, isSample);

            if (node != null)
            {
                matchNode.AppendChild(node);
            }
        }

        private void AddAllMatches(
            XmlNode matchNode,
            XmlDocument xmlDocument,
            IList<Match> matches,
            OddsFeedFilter filter)
        {
            foreach (var item in matches)
            {
                var node = item.BuildXmlNodeMatch(xmlDocument, _oddsProfile, _bookieName, filter);

                if (node != null)
                {
                    matchNode.AppendChild(node);
                }
            }
        }

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
        private XmlNode BuildOutRightEvent(XmlDocument xd, OddsFeedFilter filter, bool isOutrights, bool isSample)
        {
            var outrights = xd.CreateNode(XmlNodeType.Element, "outrights", null);

            if (_oddsProfile.OddsType.Contains(8) && _events != null)
            {
                var filteredOutrights = (from i in _events
                                         where
                                         ////filter by sports                                            
                                         (_oddsProfile.Sports == null ? true : (_oddsProfile.Sports.Count != 0 ? _oddsProfile.Sports.Contains(i.SportId) : false)) &&
                                         (filter.Sports == null || filter.Sports.Count == 0 ? true : filter.Sports.Contains(i.SportId)) &&
                                         ////filter by outrights
                                         (_oddsProfile.Outrights == null ? true : (_oddsProfile.Outrights.Count != 0 ? _oddsProfile.Outrights.Contains(i.Id) : false)) &&
                                         ////filter by leagues
                                         (filter.Leagues == null || filter.Leagues.Count == 0 ? true : FilterByLeague(filter.Leagues, i.Content[0].Content.ToLower())) &&
                                         ////filter by countries                                            
                                         (_oddsProfile.Countries == null ? true : (_oddsProfile.Countries.Count != 0 ? _oddsProfile.Countries.Contains(i.CountryCode.ToLower()) : false)) &&
                                         (filter.Countries == null || filter.Countries.Count == 0 ? true : filter.Countries.Contains(i.CountryCode.ToLower())) &&
                                         ////filter by keyword
                                         (string.IsNullOrEmpty(_oddsProfile.OutrightsKeyword) ? true : i.Content[0].Content.ToLower().Contains(_oddsProfile.OutrightsKeyword.ToLower()))
                                         select i).ToList();

                if (isSample && isOutrights && filteredOutrights.Count > 0)
                {
                    outrights.AppendChild(filteredOutrights[0].BuildXmlNodeOutRightEvent(xd, _oddsProfile));
                }
                else if (!isSample)
                {
                    foreach (var item in filteredOutrights)
                    {
                        outrights.AppendChild(item.BuildXmlNodeOutRightEvent(xd, _oddsProfile));
                    }
                }
            }

            return outrights;
        }

        private XmlNode BuildMatchEvent(XmlDocument xmlDocument, OddsFeedFilter filter, bool isSample)
        {
            var changetime = !string.IsNullOrEmpty(_timeStamp);
            int timeStamp = changetime ? int.Parse(_timeStamp) : 0;

            var deadBallMatches = GetDeadMatches(_deadballMatches, changetime, timeStamp, filter);
            var liveMatches = GetLiveMatches(_liveMatches, changetime, timeStamp, filter);

            var fullMatches = GetMatchEvent(deadBallMatches, liveMatches, isSample);

            if (fullMatches.Count > 0)
            {
                var matchNode = xmlDocument.CreateNode(XmlNodeType.Element, "matches", null);

                if (isSample)
                {
                    matchNode.AppendChild(fullMatches[0].BuildXmlNodeEvent(xmlDocument, _oddsProfile, _bookieName));
                }
                else
                {
                    BuildLeague(matchNode, xmlDocument, fullMatches);
                }

                return matchNode;
            }

            return null;
        }

        private List<Match> GetMatchEvent(IList<Match> deadballMatches, IList<Match> liveMatches, bool isSample)
        {
            var fullMatches = new List<Match>();

            if (deadballMatches.Any())
            {
                fullMatches.AddRange(deadballMatches);
            }

            if (liveMatches.Any())
            {
                if (isSample)
                {
                    if (fullMatches.Count > 0)
                    {
                        fullMatches.AddRange(liveMatches);
                    }
                }
                else
                {
                    fullMatches.AddRange(liveMatches);
                }
            }

            return fullMatches;
        }

        private void BuildLeague(XmlNode matchNode, XmlDocument xmlDocument, List<Match> fullMatches)
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

            BuildLeagueNode(matchNode, xmlDocument, leagues);
        }

        private void BuildLeagueNode(XmlNode ms, XmlDocument xd, List<League> leagues)
        {
            foreach (var lg in leagues)
            {
                var leagueNode = xd.CreateNode(XmlNodeType.Element, "league", null);
                var leagueAttId = xd.CreateAttribute("id");
                leagueAttId.Value = lg.LeagueId.ToString();
                leagueNode.Attributes.Append(leagueAttId);

                if (_oddsProfile.Languages.Contains("en-US"))
                {
                    var ge = xd.CreateAttribute("ge");
                    ge.Value = BetOddNodeBuilder.EscapeXML(lg.Matches.First().League.Where(l => l.Code == "en-US").SingleOrDefault().Content);
                    leagueNode.Attributes.Append(ge);
                }

                if (_oddsProfile.Languages.Contains("zh-TW"))
                {
                    var gs = xd.CreateAttribute("gs");
                    gs.Value = BetOddNodeBuilder.EscapeXML(lg.Matches.First().League.Where(l => l.Code == "zh-TW").SingleOrDefault().Content);
                    leagueNode.Attributes.Append(gs);
                }

                if (_oddsProfile.Languages.Contains("zh-CN"))
                {
                    var gt = xd.CreateAttribute("gt");
                    gt.Value = BetOddNodeBuilder.EscapeXML(lg.Matches.First().League.Where(l => l.Code == "zh-CN").SingleOrDefault().Content);
                    leagueNode.Attributes.Append(gt);
                }

                var leagueAttSport = xd.CreateAttribute("sport");
                leagueAttSport.Value = lg.Matches.First().SportName;
                leagueNode.Attributes.Append(leagueAttSport);

                foreach (var item in lg.Matches)
                {
                    leagueNode.AppendChild(item.BuildXmlNodeEvent(xd, _oddsProfile, _bookieName));
                }

                ms.AppendChild(leagueNode);
            }
        }

        #endregion

        private bool FilterSports(Match match, OddsFeedFilter filter)
        {
            return (_oddsProfile.Sports == null ? true : (_oddsProfile.Sports.Count != 0 ? _oddsProfile.Sports.Contains(match.SportId) : false)) &&
                    (filter.Sports == null || filter.Sports.Count == 0 ? true : filter.Sports.Contains(match.SportId));
        }

        private bool FilterLeagues(Match match, OddsFeedFilter filter)
        {
            return (_oddsProfile.Leagues == null ? true : (_oddsProfile.Leagues.Count != 0 ? _oddsProfile.Leagues.Contains(match.LeagueId) : false)) &&
                                         (filter.Leagues == null || filter.Leagues.Count == 0 ? true : FilterByLeague(filter.Leagues, match.League[0].Content.ToLower()));
        }

        private bool FilterCountries(Match match, OddsFeedFilter filter)
        {
            return (_oddsProfile.Countries == null ? true : (_oddsProfile.Countries.Count != 0 ? _oddsProfile.Countries.Contains(match.CountryCode.ToLower()) : false)) &&
                                         (filter.Countries == null || filter.Countries.Count == 0 ? true : filter.Countries.Contains(match.CountryCode.ToLower()));
        }

        private bool FilterKeywords(Match match)
        {
            return (string.IsNullOrEmpty(_oddsProfile.LeagueKeyword) ? true : match.League[0].Content.ToLower().Contains(_oddsProfile.LeagueKeyword.ToLower()));
        }
    }
}