namespace Fanex.OddsFeed.WebService.Controllers
{
    using System;
    using System.Web.Mvc;
    using WebServiceLibrary;

    public class ErrorsController : Controller
    {        
        public ActionResult Index(string msg)
        {
            return Content(ErrorBuilder.GetException(msg));
        }

        public ActionResult General()
        {
            return Content("General failure", "text/plain");
        }
    }
}
