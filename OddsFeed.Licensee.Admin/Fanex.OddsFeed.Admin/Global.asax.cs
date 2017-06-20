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
    using Fanex.Data.Configuration;
    using Fanex.Logging;
    using Fanex.OddsFeed.Admin.App_Start;
    using Fanex.OddsFeed.Core.Web.Context;
    using ONEbook.UM.Filters;
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    /// <summary>
    /// Class MVCApplication.
    /// </summary>    
    public class MvcApplication : HttpApplication
    {
        private ICurrentContext _context = new CurrentContext();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            GlobalFilters.Filters.Add(new CheckUMFilter());
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DbSettingProviderManager.Current.Start();
        }

        ////Hide this function: due to RecheckinLogin called with a interval of 5 seconds
        ////protected void Session_End(object sender, EventArgs e)
        ////{
        ////    AuthConfig.SignOut();
        ////}

        ////protected void Application_AuthenticateRequest(object sender, EventArgs e)
        ////{
        ////    //AuthConfig.RegisterAuth(Context);
        ////}

        /// <summary>
        /// Handles the Error event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError().GetBaseException();
            var httpException = ex as HttpException;
            if (httpException != null && httpException.ErrorCode == 404)
            {
                return;
            }

            LogService.Error(ex.Message, ex);
        }
    }
}