namespace Fanex.OddsFeed.WebService.Controllers
{
    using System;
    using System.Web.Mvc;
    using WebServiceLibrary;

    public class ErrorsController : Controller
    {
        public ActionResult Index()
        {
            const string msg = "Sorry for the inconvenience!We are not able to process your request this time.Please try again later";
            return Content(ErrorBuilder.GetException(msg));
        }

        public ActionResult General()
        {
            return Content("General failure", "text/plain");
        }
    }
}
