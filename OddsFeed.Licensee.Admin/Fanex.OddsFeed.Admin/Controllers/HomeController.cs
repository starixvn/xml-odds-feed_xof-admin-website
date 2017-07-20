// --------------------------------------------------------------------------
// <copyright file="HomeController.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.10.29">Style Cop</change>
//   <change who="Thai.Tran" date="2013.03.18">Add [IsAuthorize]</change>
// </history>

namespace Fanex.OddsFeed.Admin.Controllers
{
    using Fanex.OddsFeed.Core.Web.Attribute;
    using System.Web.Mvc;
    using App_Start;

    /// <summary>
    /// Class HomeController.
    /// </summary>
    [IsAuthorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
