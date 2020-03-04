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
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            const int unauthorizedStatusCode = 403;

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
                    response.StatusCode = unauthorizedStatusCode;
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
