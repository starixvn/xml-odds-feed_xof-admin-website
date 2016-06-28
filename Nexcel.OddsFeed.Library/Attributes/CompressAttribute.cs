// --------------------------------------------------------------------------
// <copyright file="CompressAttribute.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.04.03">Created</change>
// </history>

namespace Nexcel.OddsFeed.Library.Attributes
{
    using System.IO.Compression;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Class CompressAttribute.
    /// </summary>
    public class CompressAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpRequestBase request = filterContext.HttpContext.Request;
            HttpResponseBase response = filterContext.HttpContext.Response;

            var zip = request["zip"] == null ? string.Empty : request["zip"].ToLower();
            switch (zip)
            {
                case "gzip":
                    response.AppendHeader("Content-encoding", "gzip");
                    response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
                    return;
                case "deflate":
                    response.AppendHeader("Content-encoding", "deflate");
                    response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
                    return;
            }
        }
    }
}
