namespace Fanex.OddsFeed.WebService.Controllers
{
    using System.Text;
    using System.Web.Mvc;
    using WebServiceLibrary;
    using WebServiceLibrary.Helpers;

    public class ErrorsController : Controller
    {
        public ActionResult Index()
        {
            var message = SecurityHelper.EscapeXML("Sorry for the inconvenience! We are not able to process your request this time. Please try again later");

            return Content(ErrorBuilder.GetException(message), "application/xml", Encoding.UTF8);
        }

        public ActionResult General()
        {
            return Content("General failure", "text/plain");
        }
    }
}
