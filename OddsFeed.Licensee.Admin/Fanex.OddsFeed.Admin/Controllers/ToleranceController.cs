// --------------------------------------------------------------------------
// <copyright file="ToleranceController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.03.18">Add [IsAuthorize]</change>
// </history>
namespace Fanex.OddsFeed.Admin.Controllers
{
    using System.Net;
    using System.Web.Mvc;    
    using Fanex.OddsFeed.Core;

    /// <summary>
    /// Class ToleranceController.
    /// </summary>
    public class ToleranceController : BaseController
    {        
        //// GET: /Tolerance/

        #region 404 ERROR
        
        public ActionResult Error404()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return View();
        }
        #endregion

        #region UNEXPECTED ERROR
        
        public ActionResult UnexpectedError()
        {
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
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
            string startTime = string.Empty;
            string endTime = string.Empty;

            return Utilities.GetDownTime(out startTime, out endTime);
        }

        #endregion
    }
}
