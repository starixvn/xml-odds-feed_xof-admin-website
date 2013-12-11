// --------------------------------------------------------------------------
// <copyright file="AuthConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.10.29">Style Cop</change>
// </history>

namespace Fanex.OddsFeed.Admin.App_Start
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Security;        
    using Fanex.OddsFeed.Core.Criteria.User;
    using Fanex.OddsFeed.Core.Service.Impl;
    using Fanex.OddsFeed.Core.Web.Auth;
    using Fanex.OddsFeed.Core.Web.Session;

    /// <summary>
    /// Class AUTH Config.
    /// </summary>
    public class AuthConfig
    {
        /*
        public static void RegisterAuth(HttpContext context)
        {
            var user = HttpContext.Current.User;

            if (user != null)
            {
                if (user.Identity is FormsIdentity && user.Identity.IsAuthenticated)
                {
                    string cookie = FormsAuthentication.FormsCookieName;
                    HttpCookie httpCookie = context.Request.Cookies[cookie];

                    if (httpCookie == null)
                    {
                        return;
                    }

                    var ticket = FormsAuthentication.Decrypt(httpCookie.Value);
                    if (ticket == null || ticket.Expired)
                    {
                        return;
                    }

                    var identity = new FormsIdentity(ticket);
                    if (string.IsNullOrEmpty(ticket.UserData))
                    {
                        return;
                    }

                    var udata = AuthInfo.CreateUserData(ticket.UserData);
                    var principal = new NapPrincipal(identity, udata);
                    context.User = principal;
                }
            }
        }
        */

        /*
        public static void SignOut()
        {
            var _currentSession = new CurrentSession();
            var _userService = new UserService();
            var logOutUserCriteria = new LogOutUserCriteria();

            logOutUserCriteria.OnUserId = _currentSession.OnUserId;
            _userService.UserLogOut(logOutUserCriteria);
            _currentSession.ClearAll();
        }
        */
    }
}