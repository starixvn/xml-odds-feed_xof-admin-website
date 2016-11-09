namespace Fanex.OddsFeed.Library.UnitTests.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Library.Mapping;
    using Model;
    using NUnit.Framework;
    using Schemas;

    [TestFixture]
    public class MatchMappingTest
    {
        private MatchMapping _target;

        [SetUp]
        public void Setup()
        {
            _target = new MatchMapping();
        }

        [Test]
        public void GetMatchNodes_NormalOdds_ReturnCorrectNormalMatchNode()
        {
            // Arrange
            var normalDataTable = GetNormalDataTable();
            var schemas = GetSchemas();

            // Action
            var result = _target.GetMatchNodes(normalDataTable, schemas, false, null).First();

            // Assert
            Assert.AreEqual(normalDataTable.Rows[0]["MatchId"], result.MatchId);
            Assert.AreEqual(normalDataTable.Rows[0]["BetType"], result.BetTypeId);
            Assert.AreEqual(normalDataTable.Rows[0]["Odds1A"], result.BookieNode.OddsNodes[0].BetChoices.ElementAt(0).Code);
            Assert.AreEqual(normalDataTable.Rows[0]["Odds2A"], result.BookieNode.OddsNodes[0].BetChoices.ElementAt(1).Code);
        }

        [Test]
        public void GetMatchNodes_HandicapOdds_ReturnCorrectHandicapMatchNode()
        {
            // Arrange
            var handicapDataTable = GetHandicapDataTable();
            var schemas = GetSchemas();

            // Action
            var result = _target.GetMatchNodes(handicapDataTable, schemas, false, null).First();

            // Assert
            Assert.AreEqual(handicapDataTable.Rows[0]["MatchId"], result.MatchId);
            Assert.AreEqual(handicapDataTable.Rows[0]["BetType"], result.BetTypeId);
            Assert.AreEqual(handicapDataTable.Rows[0]["Odds1A"], result.BookieNode.OddsNodes[0].BetChoices.ElementAt(1).Code);
            Assert.AreEqual(handicapDataTable.Rows[0]["Odds2A"], result.BookieNode.OddsNodes[0].BetChoices.ElementAt(2).Code);
        }

        [Test]
        public void GetMatchNodes_FlexOdds_ReturnCorrectFlexOddsMatchNode()
        {
            // Arrange
            var oddsFlexDataTable = GetOddsFlexDataTable();
            var flexOdds = GetOddsFlex();
            var schemas = GetSchemas();

            // Action
            var result = _target.GetMatchNodes(oddsFlexDataTable, schemas, false, null).First();

            // Assert
            Assert.AreEqual(oddsFlexDataTable.Rows[0]["MatchId"], result.MatchId);
            Assert.AreEqual(oddsFlexDataTable.Rows[0]["BetType"], result.BetTypeId);
            Assert.AreEqual(flexOdds.ElementAt(0).Odds, result.BookieNode.OddsNodes[0].BetChoices.ElementAt(0).Code);
            Assert.AreEqual(flexOdds.ElementAt(1).Odds, result.BookieNode.OddsNodes[0].BetChoices.ElementAt(1).Code);
        }

        private IEnumerable<BetTypeSchema> GetSchemas()
        {
            return new[]
            {
                new BetTypeSchema
                {
                    BetTypeId = 1,
                    Tag = "ah",
                    Reserve = new ReserveSchema
                    {
                        FirstHdp = "Hdp1",
                        SecondHdp = "Hdp2"
                    },
                    BetChoices = new [] 
                    {
                        new BetChoiceSchema
                        {
                            MatchAttribute = "h",
                            NotConvert = true
                        },
                        new BetChoiceSchema
                        {
                            MatchAttribute = "o",
                            DataAttribute = "Odds1A"
                        },
                        new BetChoiceSchema
                        {
                            MatchAttribute = "u",
                            DataAttribute = "Odds2A"
                        }
                    }
                },
                new BetTypeSchema
                {
                    BetTypeId = 2,
                    Tag = "oe",
                    BetChoices = new []
                    {
                        new BetChoiceSchema
                        {
                            MatchAttribute = "o",
                            DataAttribute = "Odds1A"
                        },
                        new BetChoiceSchema
                        {
                            MatchAttribute = "e",
                            DataAttribute = "Odds2A"
                        }
                    }
                },
                new BetTypeSchema
                {
                    BetTypeId = 3,
                    Tag = "cs_12",
                    IsOddsFlex = true,
                    BetChoices = new [] 
                    {
                        new BetChoiceSchema
                        {
                            MatchAttribute = "O01",
                            DataAttribute = "0:0"
                        },
                        new BetChoiceSchema
                        {
                            MatchAttribute = "O02",
                            DataAttribute = "0:1"
                        }
                    }
                }
            };
        }

        private DataTable GetHandicapDataTable()
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add(new DataColumn
            {
                DataType = typeof(int),
                ColumnName = "MatchID"
            });
            dataTable.Columns.Add(new DataColumn
            {
                DataType = typeof(int),
                ColumnName = "BetType"
            });
            dataTable.Columns.Add(new DataColumn
            {
                DataType = typeof(decimal),
                ColumnName = "Hdp1"
            });
            dataTable.Columns.Add(new DataColumn
            {
                DataType = typeof(decimal),
                ColumnName = "Hdp2"
            });
            dataTable.Columns.Add(new DataColumn
            {
                DataType = typeof(decimal),
                ColumnName = "Odds1A"
            });
            dataTable.Columns.Add(new DataColumn
            {
                DataType = typeof(decimal),
                ColumnName = "Odds2A"
            });
            dataTable.Columns.Add(new DataColumn
            {
                DataType = typeof(DateTime),
                ColumnName = "ChangeTime"
            });
            dataTable.Columns.Add(new DataColumn
            {
                DataType = typeof(int),
                ColumnName = "TimeStamp"
            });

            var row = dataTable.NewRow();
            row["MatchID"] = 1;
            row["BetType"] = 1;
            row["Hdp1"] = 1;
            row["Hdp2"] = 1.5;
            row["Odds1A"] = 0.8;
            row["Odds2A"] = 0.9;
            row["ChangeTime"] = "11/01/2016 6:11:32 AM";
            row["TimeStamp"] = 123456;

            dataTable.Rows.Add(row);

            return dataTable;
        }

        private DataTable GetNormalDataTable()
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add(new DataColumn
            {
                DataType = typeof(int),
                ColumnName = "MatchID"
            });
            dataTable.Columns.Add(new DataColumn
            {
                DataType = typeof(int),
                ColumnName = "BetType"
            });
            dataTable.Columns.Add(new DataColumn
            {
                DataType = typeof(decimal),
                ColumnName = "Odds1A"
            });
            dataTable.Columns.Add(new DataColumn
            {
                DataType = typeof(decimal),
                ColumnName = "Odds2A"
            });
            dataTable.Columns.Add(new DataColumn
            {
                DataType = typeof(DateTime),
                ColumnName = "ChangeTime"
            });
            dataTable.Columns.Add(new DataColumn
            {
                DataType = typeof(int),
                ColumnName = "TimeStamp"
            });

            var row = dataTable.NewRow();
            row["MatchID"] = 1;
            row["BetType"] = 2;
            row["Odds1A"] = 0.8;
            row["Odds2A"] = 0.9;
            row["ChangeTime"] = "11/01/2016 6:11:32 AM";
            row["TimeStamp"] = 123456;

            dataTable.Rows.Add(row);

            return dataTable;
        }

        private DataTable GetOddsFlexDataTable()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn
            {
                DataType = typeof(int),
                ColumnName = "MatchID"
            });
            dataTable.Columns.Add(new DataColumn
            {
                DataType = typeof(int),
                ColumnName = "BetType"
            });
            dataTable.Columns.Add(new DataColumn
            {
                DataType = typeof(DateTime),
                ColumnName = "ChangeTime"
            });
            dataTable.Columns.Add(new DataColumn
            {
                DataType = typeof(int),
                ColumnName = "TimeStamp"
            });

            var row = dataTable.NewRow();
            row["MatchID"] = 3;
            row["BetType"] = 3;
            row["ChangeTime"] = "11/01/2016 6:11:32 AM";
            row["TimeStamp"] = 123456;

            dataTable.Rows.Add(row);

            return dataTable;
        }

        private IEnumerable<OddFlex> GetOddsFlex()
        {
            return new[]
            {
                new OddFlex { OddId = 1, Odds = 1, BetType = 3, BetChoice = "0:0" },
                new OddFlex { OddId = 2, Odds = 4, BetType = 3, BetChoice = "0:1" }
            };
        }
    }
}
