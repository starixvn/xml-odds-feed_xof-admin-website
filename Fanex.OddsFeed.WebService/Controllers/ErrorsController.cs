namespace Fanex.OddsFeed.WebService.Controllers
{
    using System;
    using System.Web.Mvc;
    using OddsFeed.Common.DTO;
    using OddsFeed.Common.Helpers;

    public class ErrorsController : Controller
    {        
        public ActionResult Index(Exception exception)
        {
            return Content(SerializerHelper.SerializeObject(
            new Error
            {
                Id = 4,
                Message = exception.Message
            }));
        }

        public ActionResult General()
        {
            return Content("General failure", "text/plain");
        }
    }
}
