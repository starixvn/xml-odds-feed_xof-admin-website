namespace Fanex.OddsFeed.Controllers
{
    using Fanex.OddsFeed.Core.Attribute;
    using Microsoft.AspNetCore.Mvc;

    [IsAuthorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}