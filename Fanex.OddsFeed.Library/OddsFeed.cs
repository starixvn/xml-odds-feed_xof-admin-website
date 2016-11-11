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
            
            return nodeRoot;
        }

        private XmlDocument CreateXmlDocument()
        {
            var xmlDocument = new XmlDocument();
            var xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);

            xmlDocument.InsertBefore(xmlDeclaration, xmlDocument.DocumentElement);

            return xmlDocument;
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