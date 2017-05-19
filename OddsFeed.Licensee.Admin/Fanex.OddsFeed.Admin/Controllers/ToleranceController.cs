// --------------------------------------------------------------------------
// <copyright file="ToleranceController.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.03.18">Add [IsAuthorize]</change>
// </history>
namespace Fanex.OddsFeed.Admin.Controllers
{
    using Fanex.OddsFeed.Core;
    using System.Web.Mvc;

    /// <summary>
    /// Class ToleranceController.
    /// </summary>
    public class ToleranceController : BaseController
    {
        //// GET: /Tolerance/

        #region 404 ERROR

        public ActionResult Error404()
        {
            return View();
        }
        #endregion

        #region UNEXPECTED ERROR

        public ActionResult UnexpectedError()
        {
            return View();
        }
        #endregion

        #region UNDER MAINTAINANCE

        public ActionResult UnderMaintainance(string timeFrom = "", string timeTo = "")
        {
            ViewBag.UmtFrom = timeFrom;
            ViewBag.UmtTo = timeTo;

            return View();
        }

        public bool IsUnderMaintainance()
        {
            var startTime = string.Empty;
            var endTime = string.Empty;

            return Utilities.GetDownTime(out startTime, out endTime);
        }

        #endregion
    }
}
