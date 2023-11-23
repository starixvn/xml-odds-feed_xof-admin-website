namespace Fanex.OddsFeed.Controllers
{
    using System.Diagnostics;
    using Fanex.OddsFeed.Core.Context;
    using Fanex.OddsFeed.Models;
    using Microsoft.AspNetCore.Mvc;
    public class HomeController : Controller
    {
        protected readonly ICurrentContext currentContext;

        public HomeController( ICurrentContext currentContext)
        {
            this.currentContext = currentContext;
        }

        public IActionResult Index()
        {
            var test = Request.Headers["User-Agent"].ToString();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}