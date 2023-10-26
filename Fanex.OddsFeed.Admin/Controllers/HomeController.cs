using System.Diagnostics;
using Fanex.OddsFeed.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Fanex.OddsFeed.Core.Web.Context;

namespace Fanex.OddsFeed.Admin.Controllers
{
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