// --------------------------------------------------------------------------
// <copyright file="OddsRouteHandler.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Nexcel.OddsFeed.Library.Routing
{
    using System.Web;
    using System.Web.Compilation;
    using System.Web.Routing;

    /// <summary>
    /// Class OddsRouteHandler.
    /// </summary>
    public class OddsRouteHandler : IRouteHandler
    {
        #region IRouteHandler Members

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            var username = requestContext.RouteData.Values["username"].ToString();
            ////var timestamp = requestContext.RouteData.Values["timestamp"].ToString();

            var context = HttpContext.Current;
            context.Items.Add("username", username);
            ////context.Items.Add("timestamp", timestamp);

            return BuildManager.CreateInstanceFromVirtualPath("~/Odds.ashx", typeof(IHttpHandler)) as IHttpHandler;
        }

        #endregion
    }
}
