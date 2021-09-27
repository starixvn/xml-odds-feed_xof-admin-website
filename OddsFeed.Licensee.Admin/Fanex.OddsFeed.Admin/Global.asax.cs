// --------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.10.29">Style Cop</change>
// </history>

namespace Fanex.OddsFeed.Admin
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Fanex.Data.Configuration;
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.Logging.Extensions.Web;
    using Fanex.Logging.Sentry.RavenClient;
    using Fanex.OddsFeed.Admin.App_Start;
    using ONEbook.UM.Filters;
    using SharpRaven;

    /// <summary>
    /// Class MVCApplication.
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            LogManager
                   .SetDefaultLogCategory(ConfigurationManager.AppSettings["SentryDNS"])
                   .Use(new SentryLogging(new SentryEngineOptions
                   {
                       Dsn = new Dsn(ConfigurationManager.AppSettings["SentryDNS"])
                   }));

            ExceptionHandler
                .UseConfig(new LoggingConfiguration())
                .UseGetDbExceptionInfo(DatabaseInfoGetters.GetDbExceptionInfo);

            AreaRegistration.RegisterAllAreas();

            GlobalFilters.Filters.Add(new CheckUMFilter());
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DbSettingProviderManager.Current.Start();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError().GetBaseException();
            ExceptionHandler.Handle(ex, TraceEventType.Error);
        }
    }
}