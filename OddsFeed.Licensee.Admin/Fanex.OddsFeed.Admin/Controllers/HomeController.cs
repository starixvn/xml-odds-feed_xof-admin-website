namespace Fanex.OddsFeed.Admin.Controllers
{
    using Fanex.OddsFeed.Core.Web.Attribute;
    using System.Web.Mvc;

    [IsAuthorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
