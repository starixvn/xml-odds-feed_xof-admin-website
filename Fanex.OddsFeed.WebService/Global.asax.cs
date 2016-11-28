// --------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Fanex.OddsFeed.WebService
{
    using Fanex.Logging;
    using log4net.Config;
    using Fanex.OddsFeed.Library;
    using Fanex.OddsFeed.Library.Attributes;
    using Fanex.OddsFeed.WebService.Controllers;
    using System;
    using System.Configuration;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    //// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    //// visit http://go.microsoft.com/?LinkId=9394801

    /// <summary>
    /// Class MVCApplication.
    /// </summary>        
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CompressAttribute());
        }

        /// <summary>
        /// Register Routes.
        /// </summary>
        /// <param name="routes">Route Collection.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            var _feedUrlPath = ConfigurationManager.AppSettings["FeedUrlPath"].ToString();
            if (!string.IsNullOrEmpty(_feedUrlPath))
            {
                _feedUrlPath += "/";
            }

            RegisterRoutesBetRadar(routes, _feedUrlPath);

            RegisterRoutesBedDaq(routes, _feedUrlPath);

            RegisterRoutesOddsFeed(routes, _feedUrlPath);

            RegisterRoutesSportsBetTypesLeaguesFeed(routes, _feedUrlPath);

            RegisterRoutesEvent(routes, _feedUrlPath);

            RegisterRoutesFeeds(routes, _feedUrlPath);

            routes.MapRoute(
                "OddsFeedWindowService",
                _feedUrlPath + "{username}/analytics/cache",
                new { controller = "Analytics", action = "Cache" });

            routes.MapRoute(
                "Default", // Route name
                _feedUrlPath + "{controller}/{action}/{id}", // URL with parameters               
               new { controller = "Home", action = "Index", id = UrlParameter.Optional }); // Parameter defaults            
        }

        /// <summary>
        /// Application_s the start.
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            Fanex.Data.Configuration.DbSettingProviderManager.Current.Start();

            XmlConfigurator.Configure();
        }

        /// <summary>
        /// Application Error.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event args.</param>
        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            Response.Clear();
            Server.ClearError();

            ////Send & log exception email
            if (ex != null && !(ex is HttpException) && !(ex is FormatException))
            {
                ////write log
                var startTime = string.Empty;
                var endTime = string.Empty;
                if (!Utilities.GetDownTime(out startTime, out endTime))
                {
                    var message = new string[]
                        {
                            "ENVIRONMENT: " + ConfigurationManager.AppSettings["Environment"].ToString(),
                            "ERROR: " + ex.Message,
                            "SOURCE: " + ex.Source,
                            "TARGETSITE: " + ex.TargetSite,
                            "STACKTRACE: " + ex.StackTrace
                        };
                    LogService.Error(string.Join(Environment.NewLine, message));
                }
            }

            ////Show friendly errors
            if (ex is HttpException || ex is FormatException)
            {
                Response.Redirect("~/PageNotFound.html");
            }
            else
            {
                var err = new Exception("Sorry for the inconvenience! We are not able to process your request this time. Please try again later");
                var routeData = new RouteData();
                routeData.Values["controller"] = "Errors";
                routeData.Values["action"] = "Index";
                routeData.Values["exception"] = err;

                IController errorsController = new ErrorsController();
                var rc = new RequestContext(new HttpContextWrapper(Context), routeData);
                errorsController.Execute(rc);
            }
        }

        #region Private register routes methods

        /// <summary>
        /// Registers the routes event.
        /// </summary>
        /// <param name="routes">The routes.</param>
        /// <param name="_feedUrlPath">The _feed URL path.</param>
        private static void RegisterRoutesEvent(RouteCollection routes, string _feedUrlPath)
        {
            routes.MapRoute(
               "EventService",
               _feedUrlPath + "{username}/event.xml",
               new { controller = "Event", action = "Index" });
        }

        /// <summary>
        /// Registers the routes feeds.
        /// </summary>
        /// <param name="routes">The routes.</param>
        /// <param name="_feedUrlPath">The _feed URL path.</param>
        private static void RegisterRoutesFeeds(RouteCollection routes, string _feedUrlPath)
        {
            routes.MapRoute(
              "FeedsCsvService",
              _feedUrlPath + "{username}/{feedname}.csv",
              new { controller = "Feeds", action = "GetCsv" });

            routes.MapRoute(
              "FeedsXmlService",
              _feedUrlPath + "{username}/{feedname}.xml",
              new { controller = "Feeds", action = "Index" });

            routes.MapRoute(
                 "FeedsJsonService",
                 _feedUrlPath + "{username}/{feedname}.json",
                 new { controller = "Feeds", action = "GetJson" });
        }

        /// <summary>
        /// Registers the routes odds feed.
        /// </summary>
        /// <param name="routes">The routes.</param>
        /// <param name="_feedUrlPath">The _feed URL path.</param>
        private static void RegisterRoutesOddsFeed(RouteCollection routes, string _feedUrlPath)
        {
            routes.MapRoute(
                "OddsFeedService",
                _feedUrlPath + "{username}/odds.xml",
                new { controller = "OddsFeed", action = "Index" });

            routes.MapRoute(
                "OddsFeedHelp",
                _feedUrlPath + "{username}/help.htm",
                new { controller = "OddsFeed", action = "Help" });

            routes.MapRoute(
              "OddsFeedOldHelp",
              _feedUrlPath + "{username}/oldhelp.htm",
              new { controller = "OddsFeed", action = "OldHelp" });
        }

        /// <summary>
        /// Registers the routes bed DAQ.
        /// </summary>
        /// <param name="routes">The routes.</param>
        /// <param name="_feedUrlPath">The _feed URL path.</param>
        private static void RegisterRoutesBedDaq(RouteCollection routes, string _feedUrlPath)
        {
            routes.MapRoute(
                "CommonOddsFeedService", // Route name
                _feedUrlPath + "{username}/odds.txt", // URL with parameters               
               new { controller = "BetDaq", action = "GetOdds" }); // Parameter defaults

            routes.MapRoute(
                "CommonScheduleService", // Route name
                _feedUrlPath + "{username}/sch.txt", // URL with parameters               
               new { controller = "BetDaq", action = "GetSchedule" }); // Parameter defaults

            routes.MapRoute(
                "CommonInsertBetdaqService",
                _feedUrlPath + "{username}/InsertBet.cmd",
                new { controller = "InsertBetdaq", action = "Index" });

            routes.MapRoute(
                "BetDaqOddsFeedService", // Route name
                _feedUrlPath + "{username}/en-lang/odds.txt", // URL with parameters               
               new { controller = "BetDaq", action = "GetOdds" }); // Parameter defaults            

            routes.MapRoute(
                "BetDaqScheduleService", // Route name
                _feedUrlPath + "{username}/en-lang/sch.txt", // URL with parameters               
               new { controller = "BetDaq", action = "GetSchedule" }); // Parameter defaults            

            routes.MapRoute(
                "BetDaqInsertBetdaqService",
                _feedUrlPath + "{username}/en-lang/InsertBet.cmd",
                new { controller = "InsertBetdaq", action = "Index" });
        }

        /// <summary>
        /// Registers the routes bed Radar.
        /// </summary>
        /// <param name="routes">The routes.</param>
        /// <param name="_feedUrlPath">The _feed URL path.</param>
        private static void RegisterRoutesBetRadar(RouteCollection routes, string _feedUrlPath)
        {
            routes.MapRoute(
                "BetRadarOddsFeedService", // Route name
                _feedUrlPath + "{username}/oddsfeed.xml", // URL with parameters               
               new { controller = "BetRadar", action = "Index" }); // Parameter defaults
        }

        private static void RegisterRoutesSportsBetTypesLeaguesFeed(RouteCollection routes, string _feedUrlPath)
        {
            routes.MapRoute(
                "SportService",
                _feedUrlPath + "{username}/sports.xml",
                new { controller = "Sport", action = "Index" });
            routes.MapRoute(
                "BetTypeService",
                _feedUrlPath + "{username}/bettypes.xml",
                new { controller = "BetType", action = "Index" });
            routes.MapRoute(
                "LeagueService",
                _feedUrlPath + "{username}/leagues.xml",
                new { controller = "League", action = "Index" });
        }

        #endregion
    }
}
