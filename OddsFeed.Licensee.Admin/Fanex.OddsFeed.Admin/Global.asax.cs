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
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Core.Helpers;
    using Fanex.Data.Configuration;
    using Fanex.OddsFeed.Admin.App_Start;
    using ONEbook.UM.Filters;

    /// <summary>
    /// Class MVCApplication.
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            GlobalFilters.Filters.Add(new CheckUMFilter());
            GlobalFilters.Filters.Add(new CustomHandleErrorAttribute());
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DbSettingProviderManager.Current.Start();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError().GetBaseException();
            var httpException = ex as HttpException;
            if (httpException != null && httpException.ErrorCode == 404)
            {
                Server.ClearError();
                Response.Redirect("~/Tolerance/Error404");
            }
            else
            {
                LogHelper.Log(ex);

                Response.Redirect("~/Tolerance/UnexpectedError");
            }
        }
    }
}