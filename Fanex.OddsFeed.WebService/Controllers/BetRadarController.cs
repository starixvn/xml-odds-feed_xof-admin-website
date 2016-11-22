namespace Fanex.OddsFeed.WebService.Controllers
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Mvc;
    using System.Xml;
    using MvcContrib.ActionResults;
    using Fanex.OddsFeed.Library;
    using Fanex.OddsFeed.Library.Attributes;
    using Fanex.OddsFeed.WebService.Functions;

    public class BetRadarController : Controller
    {
        #region Odds

        /// <summary>
        /// Gets the odds.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>System String.</returns>
        [Compress]
        public XmlResult Index(string userName)
        {
            if (userName.ToLower() != "betradar")
            {
                return new XmlResult(ErrorCollection.UnAuthenticated());
            }

            var startTime = string.Empty;
            var endTime = string.Empty;

            if (Utilities.GetDownTime(out startTime, out endTime))
            {
                return new XmlResult(ErrorCollection.UMDownTime(endTime));
            }
            
            return new XmlResult(GetOddsFeed(userName));
        }
        
        #endregion

        #region Private Methods

        private XmlElement GetOddsFeed(string username)
        {
            // ignore port number if it presents
            if (string.IsNullOrEmpty(username))
            {
                return ErrorCollection.ParameterSufficent();
            }

            var domain = Helper.DomainName(Request.ServerVariables);
            var ip = Utilities.GetIpAddressOfClient();
            var partner = new Partner(username, domain, ip, string.Empty);
            var err = partner.IsAuthenticated();

            if (err.Id == 0)
            {
                return BuildXmlDocument();
            }

            return err.ToXmlElement();
        }

        private XmlElement BuildXmlDocument()
        {
            // Load Data.
            var oddsFeed = DbConnect.GetBetRadar().ResultSet.Tables[0];

            // Generate XML.
            var xd = new XmlDocument();
            var commentMain = xd.CreateComment(" Date is based on Malaysian time ");
            var list = xd.CreateElement("List");
            xd.AppendChild(list);
            list.AppendChild(commentMain);
            foreach (DataRow row in oddsFeed.Rows)
            {
                var oddsObject = xd.CreateElement("OddsObject");
                oddsObject.SetAttribute("type", "regular");

                list.AppendChild(oddsObject);
                oddsObject.AppendChild(CreateNode(xd, "Sport", row["sporttypename"].ToString()));
                oddsObject.AppendChild(CreateNode(xd, "Tournament", row["leaguename"].ToString()));
                oddsObject.AppendChild(CreateNode(xd, "MatchId", row["matchid"].ToString()));
                oddsObject.AppendChild(CreateNode(xd, "HomeTeam", row["home"].ToString()));
                oddsObject.AppendChild(CreateNode(xd, "AwayTeam", row["away"].ToString()));
                oddsObject.AppendChild(CreateNode(xd, "Date", row["kickofftime"].ToString()));

                if (!row.IsNull("OddsID_5"))
                {
                    var comment = xd.CreateComment(" Threeway odds ");
                    oddsObject.AppendChild(comment);
                    oddsObject.AppendChild(OddsDataThreeway(xd, row));
                }

                if (!row.IsNull("OddsID_1"))
                {
                    var comment = xd.CreateComment(" Handicap odds ");
                    oddsObject.AppendChild(comment);
                    oddsObject.AppendChild(OddsDataHandicap(xd, row));
                }

                if (!row.IsNull("OddsID_3"))
                {
                    var comment = xd.CreateComment(" Total odds(over/under) ");
                    oddsObject.AppendChild(comment);
                    oddsObject.AppendChild(OddsDataOverUnder(xd, row));
                }
            }

            return xd.DocumentElement;
        }

        private XmlElement CreateNode(XmlDocument xd, string nodeName, string value)
        {
            var node = xd.CreateElement(nodeName);
            node.InnerText = value;
            return node;
        }

        private XmlElement OddsDataThreeway(XmlDocument xd, DataRow row)
        {
            var node = xd.CreateElement("OddsData");
            node.SetAttribute("type", "3w");
            node.AppendChild(CreateNode(xd, "AC", "3"));
            node.AppendChild(CreateNode(xd, "HomeOdds", Utilities.OddsFormat(row["com1_5"])));
            node.AppendChild(CreateNode(xd, "DrawOdds", Utilities.OddsFormat(row["comx_5"])));
            node.AppendChild(CreateNode(xd, "AwayOdds", Utilities.OddsFormat(row["com2_5"])));
            return node;
        }

        private XmlElement OddsDataHandicap(XmlDocument xd, DataRow row)
        {
            var node = xd.CreateElement("OddsData");
            node.SetAttribute("type", "handicap");
            node.AppendChild(CreateNode(xd, "AC", "1"));
            node.AppendChild(CreateNode(xd, "Handicap", row["hdp1_1"].ToString() + ":" + row["hdp2_1"].ToString()));
            node.AppendChild(CreateNode(xd, "HomeOdds", Utilities.OddsFormat(row["odds1hka_1"])));
            node.AppendChild(CreateNode(xd, "AwayOdds", Utilities.OddsFormat(row["odds2hka_1"])));
            return node;
        }

        private XmlElement OddsDataOverUnder(XmlDocument xd, DataRow row)
        {
            var node = xd.CreateElement("OddsData");
            node.SetAttribute("type", "total");
            node.AppendChild(CreateNode(xd, "AC", "1"));
            node.AppendChild(CreateNode(xd, "Total", Utilities.TotalFormat(row["Hdp1_3"])));
            node.AppendChild(CreateNode(xd, "UnderOdds", Utilities.OddsFormat(row["odds1hka_3"])));
            node.AppendChild(CreateNode(xd, "OverOdds", Utilities.OddsFormat(row["odds2hka_3"])));
            return node;
        }
        #endregion
    }
}
