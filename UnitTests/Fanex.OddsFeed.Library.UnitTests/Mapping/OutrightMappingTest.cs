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
    public class OutrightMappingTest
    {
        private OutrightMapping _target;

        [SetUp]
        public void Setup()
        {
            _target = new OutrightMapping();
        }

        [Test]
        public void GetEventNodes_OutrightOdds_ReturnOutrightNode()
        {
            // Arrange
            var dataTable = GetOutrightDataTable();

            // Action
            var result = _target.GetEventNodes(dataTable);

            // Assert
            Assert.AreEqual(dataTable.Rows[0]["LeagueID"], result.ElementAt(0).EventId);
            Assert.AreEqual(dataTable.Rows[0]["SportID"], result.ElementAt(0).SportId);
            Assert.AreEqual(dataTable.Rows[0]["OrID"], result.ElementAt(0).OutrightOddsNodes.ElementAt(0).OddsId);
            Assert.AreEqual(dataTable.Rows[0]["Odds"], result.ElementAt(0).OutrightOddsNodes.ElementAt(0).Value);
            Assert.AreEqual(dataTable.Rows[0]["EventDate"], result.ElementAt(0).Time);
        }

        private DataTable GetOutrightDataTable()
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add(new DataColumn
            {
                DataType = typeof(int),
                ColumnName = "LeagueID"
            });
            dataTable.Columns.Add(new DataColumn
            {
                DataType = typeof(int),
                ColumnName = "SportID"
            });
            dataTable.Columns.Add(new DataColumn
            {
                DataType = typeof(int),
                ColumnName = "OrID"
            });
            dataTable.Columns.Add(new DataColumn
            {
                DataType = typeof(decimal),
                ColumnName = "Odds"
            });
            dataTable.Columns.Add(new DataColumn
            {
                DataType = typeof(DateTime),
                ColumnName = "EventDate"
            });

            var row = dataTable.NewRow();
            row["LeagueID"] = 1;
            row["SportID"] = 1;
            row["OrID"] = 1;
            row["Odds"] = 1.5;
            row["EventDate"] = "11/01/2016 6:11:32 AM";

            dataTable.Rows.Add(row);

            return dataTable;
        }
    }
}
