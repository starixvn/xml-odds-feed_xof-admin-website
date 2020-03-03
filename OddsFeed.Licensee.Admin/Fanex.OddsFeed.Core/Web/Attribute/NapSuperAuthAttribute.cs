// --------------------------------------------------------------------------
// <copyright file="NapSuperAuthAttribute.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.2">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Web.Attribute
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Fanex.OddsFeed.Core.Web.Session;

    /// <summary>
    /// Class NapSuperAUTHAttribute.
    /// </summary>    
    public class NapSuperAuthAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Called when a process requests authorization.
        /// </summary>
        /// <param name="filterContext">The filter context, which encapsulates information for using <see cref="T:System.Web.Mvc.AuthorizeAttribute" />.</param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (filterContext.HttpContext == null)
            {
                return;
            }

            if (!CheckSessionAuthorization())
            {
                var response = filterContext.HttpContext.Response;

                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    response.Buffer = true;
                    response.StatusCode = 403; // Unauthorized status code
                    response.Write("<center><b>" + ViewResources.UnauthorizeMsg + "</b></center>");
                    response.Flush();
                    response.Close();
                    response.End();
                }
                else
                {
                    response.Redirect("~/userlogin/login");
                }
            }
        }

        private static bool CheckSessionAuthorization()
        {
            ICurrentSession session = new CurrentSession();

            if (string.IsNullOrEmpty(session.Username))
            {
                return false;
            }

            return true;
        }
    }
}
