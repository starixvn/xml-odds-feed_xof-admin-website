namespace Fanex.OddsFeed.WebService.Controllers
{
    using System;
    using System.Web.Mvc;
    using WebServiceLibrary;

    public class ErrorsController : Controller
    {        
        public ActionResult Index(Exception exception)
        {
            return Content(ErrorBuilder.GetException(exception.Message));
        }

        public ActionResult General()
        {
            return Content("General failure", "text/plain");
        }
    }
}
