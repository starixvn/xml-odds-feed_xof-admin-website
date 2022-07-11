namespace Fanex.OddsFeed.Admin.Controllers
{
    using Fanex.OddsFeed.Core;
    using System.Web.Mvc;

    public class ToleranceController : BaseController
    {
        public ActionResult Error404()
        {
            return View();
        }

        public ActionResult UnexpectedError()
        {
            return View();
        }

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
    }
}