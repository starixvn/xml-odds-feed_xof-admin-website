namespace Fanex.OddsFeed.Library.UnitTests.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Xml;
    using Library.Builders;
    using Library.Mapping;
    using Model;
    using Nodes;
    using NUnit.Framework;
    using Schemas;

    [TestFixture]
    public class OutrightBuilderTest
    {
        private OutrightBuilder _target;

        [SetUp]
        public void Setup()
        {
            _target = new OutrightBuilder();
        }

        [Test]
        public void CreateOutrightsNode_OutrightNodeList_ReturnXmlNode()
        {
            // Arrange
            var outrightXmlNode = GetOutrightXmlNode();
            var xmlDocument = new XmlDocument();
            var oddsProfile = new OddsProfile();
            var outrights = GetOutrightNodes();

            // Action
            var result = _target.CreateOutrightsNode(xmlDocument, oddsProfile, outrights);

            // Assert
            Assert.AreEqual(outrightXmlNode, result.OuterXml);
        }

        private string GetOutrightXmlNode()
        {
            return "<outrights>"
                + "<event id=\"9966\" ge=\"*UEFA EUROPA LEAGUE - GROUP A WINNER\" gt=\"*UEFA EUROPA LEAGUE - GROUP A WINNER\" gs=\"*UEFA EUROPA LEAGUE - GROUP A WINNER\" mt=\"12/8/2016 12:00:00 AM\" sport=\"Soccer\">"
                + "<odd id=\"7207912\" te=\"Manchester United\" tt=\"Manchester United\" ts=\"Manchester United\" o=\"0.3\" />"
                + "</event>"
                + "</outrights>";
        }

        private IEnumerable<OutrightNode> GetOutrightNodes()
        {
            return new[]
            {
                new OutrightNode
                {
                    EventId = 9966,
                    EventNames = new []
                    {
                        new Language { Code = "en-US", Content = "*UEFA EUROPA LEAGUE - GROUP A WINNER" },
                        new Language { Code = "zh-TW", Content = "*UEFA EUROPA LEAGUE - GROUP A WINNER" },
                        new Language { Code = "zh-CN", Content = "*UEFA EUROPA LEAGUE - GROUP A WINNER" }
                    },
                    SportId = 1,
                    SportName = "Soccer",
                    Time = new DateTime(2016,12,8),
                    OutrightOddsNodes = new []
                    {
                        new OutrightOddsNode
                        {
                            OddsId = 7207912,
                            Value = 0.3m,
                            Names = new []
                            {
                                new Language { Code = "en-US", Content = "Manchester United" },
                                new Language { Code = "zh-TW", Content = "Manchester United" },
                                new Language { Code = "zh-CN", Content = "Manchester United" }
                            }
                        }
                    }
                }
            };
        }
    }
}
