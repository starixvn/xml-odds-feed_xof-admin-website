namespace Fanex.OddsFeed.WebService.Controllers
{
    using System.Data;
    using System.Web.Mvc;
    using System.Xml;
    using Fanex.OddsFeed.WebService.Functions;
    using OddsFeed.Common;
    using OddsFeed.Common.Formatter;
    using WebServiceLibrary;
    using WebServiceLibrary.Criteria;

    public class BetRadarController : BaseController
    {
        public ActionResult Index(string userName)
        {
            if (userName.ToLower() != "betradar")
            {
                return Content(SerializeObject(ErrorBuilder.GetUnAuthenticated()));
            }

            if (IsDownTime())
            {
                var underMaintenanceTime = GetUndermaintenanceTime();

                return Content(SerializeObject(ErrorBuilder.GetUMDownTime(underMaintenanceTime.To)));
            }
            
            return Content(GetOddsFeed(userName));
        }

        private string GetOddsFeed(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return SerializeObject(ErrorBuilder.GetParameterSufficent());
            }

            var userProfileResult = GetAuthenticatedUserProfile(new CheckPermissionCriteria
            {
                UserName = username,
                DomainName = Helper.DomainName(Request.ServerVariables),
                Ip = GetClientIpAddress()
            });
            
            if (userProfileResult.Error.Id == 0)
            {
                return BuildXmlDocument();
            }

            return SerializeObject(userProfileResult.Error);
        }

        private string BuildXmlDocument()
        {
            var oddsFeed = DbAccess.GetBetRadar().ResultSet.Tables[0];
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
            node.AppendChild(CreateNode(xd, "HomeOdds", NumberFormatter.FormatOdds(row["com1_5"])));
            node.AppendChild(CreateNode(xd, "DrawOdds", NumberFormatter.FormatOdds(row["comx_5"])));
            node.AppendChild(CreateNode(xd, "AwayOdds", NumberFormatter.FormatOdds(row["com2_5"])));
            return node;
        }

        private XmlElement OddsDataHandicap(XmlDocument xd, DataRow row)
        {
            var node = xd.CreateElement("OddsData");
            node.SetAttribute("type", "handicap");
            node.AppendChild(CreateNode(xd, "AC", "1"));
            node.AppendChild(CreateNode(xd, "Handicap", row["hdp1_1"].ToString() + ":" + row["hdp2_1"].ToString()));
            node.AppendChild(CreateNode(xd, "HomeOdds", NumberFormatter.FormatOdds(row["odds1hka_1"])));
            node.AppendChild(CreateNode(xd, "AwayOdds", NumberFormatter.FormatOdds(row["odds2hka_1"])));
            return node;
        }

        private XmlElement OddsDataOverUnder(XmlDocument xd, DataRow row)
        {
            var node = xd.CreateElement("OddsData");
            node.SetAttribute("type", "total");
            node.AppendChild(CreateNode(xd, "AC", "1"));
            node.AppendChild(CreateNode(xd, "Total", NumberFormatter.FormatTotal(row["Hdp1_3"])));
            node.AppendChild(CreateNode(xd, "UnderOdds", NumberFormatter.FormatOdds(row["odds1hka_3"])));
            node.AppendChild(CreateNode(xd, "OverOdds", NumberFormatter.FormatOdds(row["odds2hka_3"])));
            return node;
        }
    }
}
