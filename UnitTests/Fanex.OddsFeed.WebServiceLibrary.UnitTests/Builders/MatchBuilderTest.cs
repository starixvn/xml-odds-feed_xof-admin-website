namespace Fanex.OddsFeed.WebServiceLibrary.UnitTests.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Common.DTO;
    using Models;
    using NUnit.Framework;
    using WebServiceLibrary.Builders;

    [TestFixture]
    public class MatchBuilderTest
    {
        private MatchBuilder _target;

        [SetUp]
        public void Setup()
        {
            _target = new MatchBuilder();
        }

        [Test]
        public void CreateMatchNodeList_DeadballMatchNodeList_ReturnDeadballMatchNodes()
        {
            // Arrange
            var matchXmlNode = GetDeadballMatchXmlNode();
            var oddsFeedNode = new StringBuilder();
            var deadballMatchNodes = GetMatchNodes(false);
            var liveMatchNodes = new List<Match>();
            var lastUpdate = new DateTime(2016, 11, 2, 15, 51, 07);

            var model = new OddsFeedsModel
            {
                BookieName = "111candy",
                UserProfile = new UserProfile { OddRegion = 1 },
                UserQuery = new UserQuery()
            };

            // Action
            _target.AppendMatchesNode(
                oddsFeedNode,
                deadballMatchNodes,
                liveMatchNodes,
                model,
                lastUpdate);

            // Assert
            Assert.AreEqual(matchXmlNode, oddsFeedNode.ToString());
        }

        [Test]
        public void CreateMatchNodeList_LiveMatchNodeList_ReturnLiveMatchNodes()
        {
            // Arrange 
            var matchXmlNode = GetLiveMatchXmlNode();
            var oddsFeedNode = new StringBuilder();
            var deadballMatchNodes = new List<Match>();
            var liveMatchNodes = GetMatchNodes(true);
            var lastUpdate = new DateTime(2016, 11, 2, 15, 51, 07);
            var model = new OddsFeedsModel
            {
                BookieName = "111candy",
                UserProfile = new UserProfile { OddRegion = 1 },
                UserQuery = new UserQuery()
            };

            // Action
            _target.AppendMatchesNode(
                oddsFeedNode,
                deadballMatchNodes,
                liveMatchNodes,
                model, 
                lastUpdate);

            // Assert
            Assert.AreEqual(matchXmlNode, oddsFeedNode.ToString());
        }

        private static string GetDeadballMatchXmlNode()
        {
            return "<matches lastupdate=\"11/2/2016 3:51:07 PM\" timestamp=\"1477878874\" oddstype=\"Malay\">"
                + "<match id=\"20485706\" ge=\"UEFA EUROPA LEAGUE\" gt=\"UEFA EUROPA LEAGUE\" gs=\"UEFA EUROPA LEAGUE\" he=\"FC Astana\" ht=\"FC Astana\" hs=\"FC Astana\" ae=\"Olympiakos\" at=\"Olympiakos\" as=\"Olympiakos\" mt=\"11/3/2016 11:59:59 AM\" sport=\"Soccer\" running=\"0\">"
                + "<bookie name=\"111candy\" simp=\"111candy\" trad=\"111candy\">"
                + "<odd t=\"oe\" o=\"1\" e=\"0.9\" time=\"11/2/2016 3:51:07 PM\" timestamp=\"1477332693\" />"
                + "</bookie>"
                + "</match>"
                + "</matches>";
        }

        private static string GetLiveMatchXmlNode()
        {
            return "<matches lastupdate=\"11/2/2016 3:51:07 PM\" timestamp=\"1477878874\" oddstype=\"Malay\">"
                + "<match id=\"20485706\" ge=\"UEFA EUROPA LEAGUE\" gt=\"UEFA EUROPA LEAGUE\" gs=\"UEFA EUROPA LEAGUE\" he=\"FC Astana\" ht=\"FC Astana\" hs=\"FC Astana\" ae=\"Olympiakos\" at=\"Olympiakos\" as=\"Olympiakos\" mt=\"11/3/2016 11:59:59 AM\" sport=\"Soccer\" running=\"1\">"
                + "<bookie name=\"111candy\" simp=\"111candy\" trad=\"111candy\">"
                + "<odd t=\"oe\" o=\"1\" e=\"0.9\" time=\"11/2/2016 3:51:07 PM\" timestamp=\"1477332693\" />"
                + "</bookie>"
                + "</match>"
                + "</matches>";
        }

        private static IList<Match> GetMatchNodes(bool isLive)
        {
            return new List<Match>
            {
                new Match
                {
                    MatchId = 20485706,
                    IsLive = isLive,
                    LeagueEnglish = "UEFA EUROPA LEAGUE",
                    LeagueTraditionalChinese = "UEFA EUROPA LEAGUE",
                    LeagueSimplifiedChinese = "UEFA EUROPA LEAGUE",
                    HomeEnglish = "FC Astana",
                    HomeTraditionalChinese = "FC Astana",
                    HomeSimplifiedChinese = "FC Astana",
                    AwayEnglish = "Olympiakos",
                    AwayTraditionalChinese = "Olympiakos",
                    AwaySimplifiedChinese = "Olympiakos",
                    KickOfTime = new DateTime(2016, 11, 3, 11, 59, 59),
                    SportName = "Soccer",
                    BookieNameEnglish = "12bet",
                    TimeStamp = 1477878874,
                    MatchOddsList = new []
                    {
                        new MatchOdds
                        {
                            Tag = "oe",
                            BetTypeId = 1,
                            Time = new DateTime(2016, 11, 2, 15, 51, 07),
                            TimeStamp = 1477332693,
                            BetChoices = new[]
                            {
                                new BetChoice { Code = "o", Value = "2.0" },
                                new BetChoice { Code = "e", Value = "1.9" }
                            }
                        }
                    }
                }
            };
        }
    }
}
