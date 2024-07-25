namespace Fanex.OddsFeed.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ErrorController : Controller
    {
        [Route("Error/404")]
        [Route("Error/PageNotFound")]
        public IActionResult PageNotFound()
        {
            return View();
        }

        [Route("Error/500")]
        [Route("Error/InternalError")]
        public IActionResult InternalError()
        {
            return View();
        }
    }
}