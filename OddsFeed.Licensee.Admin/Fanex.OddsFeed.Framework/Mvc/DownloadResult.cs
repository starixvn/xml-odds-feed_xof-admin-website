// --------------------------------------------------------------------------
// <copyright file="DownloadResult.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Frank.Ngo" date="2012.07.10">Created</change>
// </history>

namespace Fanex.OddsFeed.Framework.Mvc
{
    using System.IO;
    using System.Web.Mvc;

    /// <summary>
    /// <see cref="http://haacked.com/archive/2008/05/10/writing-a-custom-file-download-action-result-for-asp.net-mvc.aspx" />
    /// </summary>
    public class DownloadResult : ActionResult
    {
        public DownloadResult()
        {
        }

        public DownloadResult(string virtualPath)
        {
            this.VirtualPath = virtualPath;
        }

        public string VirtualPath
        {
            get;
            set;
        }

        public string FileDownloadName
        {
            get;
            set;
        }

        public string ErrorUrl
        {
            get;
            set;
        }

        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult" /> class.
        /// </summary>
        /// <param name="context">The context in which the result is executed.
        /// The context information includes the controller, HTTP content, request context, and route data.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (!string.IsNullOrEmpty(this.FileDownloadName))
            {
                this.FileDownloadName = this.FileDownloadName.Replace(" ", string.Empty);
                context.HttpContext.Response.AddHeader("content-disposition", "attachment; filename=" + this.FileDownloadName);
            }

            string filePath = context.HttpContext.Server.MapPath(this.VirtualPath);
            if (!File.Exists(filePath))
            {
                context.HttpContext.Response.Redirect(this.ErrorUrl);
            }
            else
            {
                context.HttpContext.Response.TransmitFile(filePath);
            }
        }
    }
}
