namespace Fanex.OddsFeed.Library.UnitTests.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using Library.Builders;
    using Nodes;
    using NUnit.Framework;

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
            var xmlDocument = new XmlDocument();
            var oddsProfile = new OddsProfile {  OddRegion = 1 };
            var matchNodes = GetMatchNodes(false);
            var bookieName = "111candy";
            var lastUpdate = new DateTime(2016, 11, 2, 15, 51, 07);
            var timeStamp = 1477878874;

            // Action
            var result = _target.CreateMatchesNode(xmlDocument, oddsProfile, matchNodes, null, bookieName, lastUpdate, timeStamp);

            // Assert
            Assert.AreEqual(matchXmlNode, result.OuterXml);
        }

        [Test]
        public void CreateMatchNodeList_LiveMatchNodeList_ReturnLiveMatchNodes()
        {
            // Arrange
            var matchXmlNode = GetLiveMatchXmlNode();
            var xmlDocument = new XmlDocument();
            var oddsProfile = new OddsProfile { OddRegion = 1 };
            var matchNodes = GetMatchNodes(true);
            var bookieName = "111candy";
            var lastUpdate = new DateTime(2016, 11, 2, 15, 51, 07);
            var timeStamp = 1477878874;

            // Action
            var result = _target.CreateMatchesNode(xmlDocument, oddsProfile, matchNodes, null, bookieName, lastUpdate, timeStamp);

            // Assert
            Assert.AreEqual(matchXmlNode, result.OuterXml);
        }

        private string GetDeadballMatchXmlNode()
        {
            return "<matches lastupdate=\"11/2/2016 3:51:07 PM\" timestamp=\"1477878874\" oddstype=\"Malay\">"
                + "<match id=\"20485706\" ge=\"UEFA EUROPA LEAGUE\" gt=\"UEFA EUROPA LEAGUE\" gs=\"UEFA EUROPA LEAGUE\" he=\"FC Astana\" ht=\"FC Astana\" hs=\"FC Astana\" ae=\"Olympiakos\" at=\"Olympiakos\" as=\"Olympiakos\" mt=\"11/3/2016 11:59:59 AM\" sport=\"Soccer\" running=\"0\">"
                + "<bookie name=\"111candy\" simp=\"111candy\" trad=\"111candy\">"
                + "<odd t=\"oe\" o=\"1\" e=\"0.9\" time=\"11/2/2016 3:51:07 PM\" timestamp=\"1477332693\" />"
                + "</bookie>"
                + "</match>"
                + "</matches>";
        }

        private string GetLiveMatchXmlNode()
        {
            return "<matches lastupdate=\"11/2/2016 3:51:07 PM\" timestamp=\"1477878874\" oddstype=\"Malay\">"
                + "<match id=\"20485706\" ge=\"UEFA EUROPA LEAGUE\" gt=\"UEFA EUROPA LEAGUE\" gs=\"UEFA EUROPA LEAGUE\" he=\"FC Astana\" ht=\"FC Astana\" hs=\"FC Astana\" ae=\"Olympiakos\" at=\"Olympiakos\" as=\"Olympiakos\" mt=\"11/3/2016 11:59:59 AM\" sport=\"Soccer\" running=\"1\">"
                + "<bookie name=\"111candy\" simp=\"111candy\" trad=\"111candy\">"
                + "<odd t=\"oe\" o=\"1\" e=\"0.9\" time=\"11/2/2016 3:51:07 PM\" timestamp=\"1477332693\" />"
                + "</bookie>"
                + "</match>"
                + "</matches>";
        }

        private IEnumerable<MatchNode> GetMatchNodes(bool isLive)
        {
            return new[]
            {
                new MatchNode
                {
                    MatchId = 20485706,
                    IsLive = isLive,
                    LeagueNames = new[]
                    {
                        new Language { Code = "en-US", Content = "UEFA EUROPA LEAGUE" },
                        new Language { Code = "zh-TW", Content = "UEFA EUROPA LEAGUE" },
                        new Language { Code = "zh-CN", Content = "UEFA EUROPA LEAGUE" }
                    },
                    HomeNames = new []
                    {
                        new Language { Code = "en-US", Content = "FC Astana" },
                        new Language { Code = "zh-TW", Content = "FC Astana" },
                        new Language { Code = "zh-CN", Content = "FC Astana" }
                    },
                    AwayNames = new []
                    {
                        new Language { Code = "en-US", Content = "Olympiakos" },
                        new Language { Code = "zh-TW", Content = "Olympiakos" },
                        new Language { Code = "zh-CN", Content = "Olympiakos" }
                    },
                    KickOfTime = new DateTime(2016, 11, 3, 11, 59, 59),
                    SportName = "Soccer",
                    BookieNode = new BookieNode
                    {
                        OddsNodes = new []
                        {
                            new MatchOddsNode
                            {
                                Tag = "oe",
                                BetTypeId = 1,
                                Time = new DateTime(2016, 11, 2, 15, 51, 07),
                                TimeStamp = 1477332693,
                                BetChoices = new[]
                                {
                                    new BetChoice { Code = "o", Value = "1.0" },
                                    new BetChoice { Code = "e", Value = "0.9" }
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}
