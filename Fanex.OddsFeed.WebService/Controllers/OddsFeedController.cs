namespace Fanex.OddsFeed.WebService.Controllers
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Text;
    using System.Web.Mvc;
    using System.Xml;
    using log4net;
    using MvcContrib.ActionResults;
    using Fanex.OddsFeed.WebService.Functions;
    using WebServiceLibrary.Services;
    using OddsFeed.Common;
    using OddsFeed.Common.Configurations;
    using WebServiceLibrary;
    using OddsFeed.Common.DTO;

    public class OddsFeedController : BaseController
    {
        private readonly ILog _logService;
        private readonly IBetTypeService _betTypeService;

        public OddsFeedController()
        {
            _logService = LogManager.GetLogger("LogService");
            _betTypeService = new BetTypeService();
        }

        public XmlResult Index(string username)
        {
            _logService.Info("1. Starting Index action:" + username + "-----" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"));
            
            if (IsDownTime())
            {
                var underMaintenanceTime = GetUndermaintenanceTime();

                return new XmlResult(ErrorBuilder.GetUMDownTime(underMaintenanceTime.To));
            }

            var filter = Helper.GetFilter(Request);
            if (Helper.IsOverLimitedOptions(filter))
            {
                var errMessage = new Error { Id = 10024, Message = "You can only filter maximum 5 options. Please try again." };
                return new XmlResult(errMessage);
            }

            UserProfile userProfile = null;
            var domain = Helper.DomainName(Request.ServerVariables);
            var data = GetOddsFeed(ref userProfile, username, HttpContext.Request["t"], domain, filter);

            _logService.Info("4. Finishing Index action:" + username + "------" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"));
            _logService.Info("-------------------------------");
            return new XmlResult(data);
        }

        public ActionResult OldHelp(string username)
        {
            if (IsDownTime())
            {
                var underMaintenanceTime = GetUndermaintenanceTime();

                return new XmlResult(ErrorBuilder.GetUMDownTime(underMaintenanceTime.To));
            }

            ViewBag.UserName = username;
            ViewBag.FullDomainName = Helper.FullDomainName(Request.ServerVariables);
            ViewBag.VirtualPath = GetVirtualPath();

            UserProfile userProfile = null;
            var domain = Helper.DomainName(Request.ServerVariables);
            var userQuery = new UserQuery();
            var sampleOdds = GetOddsFeed(ref userProfile, username, HttpContext.Request["t"], domain, userQuery, true, true);

            var provider = new StringBuilder(sampleOdds.OuterXml);
            provider.Replace("><", ">\n<");
            provider.Replace(" />", "></odd>");
            provider.Replace("<outrights>", "\t<outrights>")
                .Replace("<event", "\t\t<event")
                .Replace("<odd id", "\t\t\t<odd id")
                .Replace("</event>", "\t\t</event>")
                .Replace("</outrights>", "\t</outrights>")

                .Replace("<matches", "\t<matches")
                .Replace("<match id", "\t\t<match id")
                .Replace("<bookie", "\t\t\t<bookie")
                .Replace("<odd t", "\t\t\t\t<odd t")
                .Replace("</bookie", "\t\t\t</bookie")
                .Replace("</match>", "\t\t</match>")
                .Replace("</matches>", "\t</matches>");

            ViewBag.Odds = provider.ToString();

            ViewBag.Sports = DbAccess.GetSportByUserName(username).ResultSet.Tables[0].Select();
            ViewBag.Bettypes = DbAccess.GetBetTypeByUserName(username).ResultSet.Tables[0].Select();

            if (userProfile != null)
            {
                return View("Help", userProfile);
            }
            else
            {
                return new XmlResult(sampleOdds);
            }
        }

        public ActionResult Help(string username)
        {
            if (IsDownTime())
            {
                var underMaintenanceTime = GetUndermaintenanceTime();

                return new XmlResult(ErrorBuilder.GetUMDownTime(underMaintenanceTime.To));
            }

            ViewBag.UserName = username;
            ViewBag.FullDomainName = Helper.FullDomainName(Request.ServerVariables);
            ViewBag.VirtualPath = GetVirtualPath();

            UserProfile userProfile = null;
            var domain = Helper.DomainName(Request.ServerVariables);
            var userQuery = new UserQuery();
            var sampleOdds = GetOddsFeed(ref userProfile, username, HttpContext.Request["t"], domain, userQuery, true, true);

            var provider = new StringBuilder(sampleOdds.OuterXml);
            provider.Replace("><", ">\n<");
            provider.Replace(" />", "></odd>");
            provider.Replace("<outrights>", "\t<outrights>")
                .Replace("<event", "\t\t<event")
                .Replace("<odd id", "\t\t\t<odd id")
                .Replace("</event>", "\t\t</event>")
                .Replace("</outrights>", "\t</outrights>")

                .Replace("<matches", "\t<matches")
                .Replace("<match id", "\t\t<match id")
                .Replace("<bookie", "\t\t\t<bookie")
                .Replace("<odd t", "\t\t\t\t<odd t")
                .Replace("</bookie", "\t\t\t</bookie")
                .Replace("</match>", "\t\t</match>")
                .Replace("</matches>", "\t</matches>");

            ViewBag.Odds = provider.ToString();

            ViewBag.ContactEmail = DbAccess.GetContactEmailByUserName(username);

            GenerateAppendixData(username);

            if (userProfile != null)
            {
                return View("NewHelp", userProfile);
            }
            else
            {
                return new XmlResult(sampleOdds);
            }
        }

        private void GenerateAppendixData(string username)
        {
            //// Generate supported sports
            var sportsTable = DbAccess.GetSportByUserName(username).ResultSet.Tables[0];
            var sportsCol1 = new DataTable();
            var sportsCol2 = new DataTable();
            var sportsCol3 = new DataTable();
            GenerateData(sportsTable, sportsCol1, sportsCol2, sportsCol3, "SportID", "SportName");
            ViewBag.SportsCol1 = sportsCol1.Select();
            ViewBag.SportsCol2 = sportsCol2.Select();
            ViewBag.SportsCol3 = sportsCol3.Select();

            //// Generate supported bettypes
            var rawBettypesTable = DbAccess.GetBetTypeByUserName(username).ResultSet.Tables[0];
            var bettypesTable = SortDataTable(rawBettypesTable, "BetTypeID", "ASC");
            if (bettypesTable.Rows.Count == 0)
            {
                //// Show Outright bettype (It's an exception)
                var row = bettypesTable.NewRow();
                row["BetTypeID"] = 10;
                row["BetTypeName"] = "Outright";
                bettypesTable.Rows.Add(row);
            }

            var bettypesCol1 = new DataTable();
            var bettypesCol2 = new DataTable();
            var bettypesCol3 = new DataTable();
            GenerateData(bettypesTable, bettypesCol1, bettypesCol2, bettypesCol3, "BetTypeID", "BetTypeName");
            ViewBag.BetTypesCol1 = bettypesCol1.Select();
            ViewBag.BetTypesCol2 = bettypesCol2.Select();
            ViewBag.BetTypesCol3 = bettypesCol3.Select();

            //// Generate supported countries
            var countriesTable = DbAccess.Odds_CountrySelByUserName(username).ResultSet.Tables[0];
            var countriesCol1 = new DataTable();
            var countriesCol2 = new DataTable();
            var countriesCol3 = new DataTable();
            GenerateData(countriesTable, countriesCol1, countriesCol2, countriesCol3, "CountryID", "CountryName");
            ViewBag.CountriesCol1 = countriesCol1.Select();
            ViewBag.CountriesCol2 = countriesCol2.Select();
            ViewBag.CountriesCol3 = countriesCol3.Select();
            ViewBag.HighlightSportType = AppSettings.HighlightSportType.Split(',');
            ViewBag.HighlightBetType = AppSettings.HighlightBetType.Split(',');
            ViewBag.BetTypeList = _betTypeService.GetBetTypeList();
        }

        public static DataTable SortDataTable(DataTable dt, string colName, string direction)
        {
            DataTable dtOut = null;
            dt.DefaultView.Sort = colName + " " + direction;
            dtOut = dt.DefaultView.ToTable();
            return dtOut;
        }

        private static void GenerateData(DataTable dt, DataTable column1Data, DataTable column2Data, DataTable column3Data, string columnID, string columnName)
        {
            var sum = dt.Rows.Count;
            if (sum <= 5)
            {
                GenerateColumnData(dt, column1Data, column2Data, column3Data, columnID, columnName, sum, 0, 0);
            }
            else
            {
                var divide = sum / 3;
                var module = sum % 3;
                var sumCol1 = divide;
                var sumCol2 = divide;
                var sumCol3 = divide;

                while (module > 0)
                {
                    sumCol2 += 1;
                    module--;

                    if (module > 0)
                    {
                        sumCol3 += 1;
                        module--;
                    }

                    if (module > 0)
                    {
                        sumCol1 += 1;
                        module--;
                    }
                }

                GenerateColumnData(dt, column1Data, column2Data, column3Data, columnID, columnName, sumCol1, sumCol2, sumCol3);
            }
        }

        private static void GenerateColumnData(
            DataTable dt,
            DataTable column1Data,
            DataTable column2Data,
            DataTable column3Data,
            string columnID,
            string columnName,
            int sumCol1,
            int sumCol2,
            int sumCol3)
        {
            ////Col1            
            column1Data.Columns.Add(columnID, typeof(string));
            column1Data.Columns.Add(columnName, typeof(string));
            var i = 0;
            while (i < sumCol1)
            {
                var row = column1Data.NewRow();
                row[columnID] = dt.Rows[i][columnID].ToString();
                row[columnName] = dt.Rows[i][columnName].ToString();
                column1Data.Rows.Add(row);
                i++;
            }

            ////Col2
            column2Data.Columns.Add(columnID, typeof(string));
            column2Data.Columns.Add(columnName, typeof(string));
            while (i < sumCol1 + sumCol2)
            {
                var row = column2Data.NewRow();
                row[columnID] = dt.Rows[i][columnID].ToString();
                row[columnName] = dt.Rows[i][columnName].ToString();
                column2Data.Rows.Add(row);
                i++;
            }

            ////Col3            
            column3Data.Columns.Add(columnID, typeof(string));
            column3Data.Columns.Add(columnName, typeof(string));
            while (i < sumCol1 + sumCol2 + sumCol3)
            {
                var row = column3Data.NewRow();
                row[columnID] = dt.Rows[i][columnID].ToString();
                row[columnName] = dt.Rows[i][columnName].ToString();
                column3Data.Rows.Add(row);
                i++;
            }
        }

        private XmlElement GetOddsFeed(
            ref UserProfile userProfile,
            string username,
            string timestamp,
            string domain,
            UserQuery userQuery,
            bool isSample = false,
            bool outrights = true)
        {
            // ignore port number if it presents
            var domainName = domain;

            if (string.IsNullOrEmpty(username))
            {
                return ErrorCollection.ParameterSufficent();
            }

            var ipAddress = GetClientIpAddress();
            var partner = new Partner(username, domainName, ipAddress, timestamp);
            var err = partner.IsAuthenticated();

            if (partner.UserProfile != null && (err.Id == 0 || isSample))
            {
                userProfile = partner.UserProfile;
                userProfile.Domain = domainName;

                _logService.Info("2. Get Odds from Cache of " + username + "---" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"));
                var service = new WebOddsFeedService(partner.UserProfile, partner.SystemName, domainName, ipAddress, timestamp);
                _logService.Info("3. Build XML Document " + username + "---" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"));
                var xmlDocument = service.BuildXmlDocument(partner.Timestamp, userQuery, isSample, outrights);

                return xmlDocument;
            }

            return err.ToXmlElement();
        }
        
        private static string GetVirtualPath()
        {
            var feedUrlPath = ConfigurationManager.AppSettings["FeedUrlPath"].ToString();
            if (!string.IsNullOrEmpty(feedUrlPath))
            {
                feedUrlPath += "/";
            }

            var virtualPathWebService = ConfigurationManager.AppSettings["VirtualPathWebService"].ToString();
            if (!string.IsNullOrEmpty(virtualPathWebService))
            {
                virtualPathWebService += "/";
            }

            return virtualPathWebService + feedUrlPath;
        }
    }
}
