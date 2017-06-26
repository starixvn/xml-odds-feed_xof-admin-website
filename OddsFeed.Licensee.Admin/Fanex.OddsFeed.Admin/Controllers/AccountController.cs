// --------------------------------------------------------------------------
// <copyright file="AccountController.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Admin.Controllers
{
    using System.Web.Mvc;
    using App_Start;

    [CustomHandleError]
    public class AccountController : Controller
    {
        //// GET: /Account/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
    }
}
