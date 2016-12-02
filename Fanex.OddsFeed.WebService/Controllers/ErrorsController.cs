namespace Fanex.OddsFeed.WebService.Controllers
{
    using System;
    using System.Web.Mvc;
    using MvcContrib.ActionResults;
    using OddsFeed.Common.DTO;

    public class ErrorsController : Controller
    {        
        public XmlResult Index(Exception exception)
        {
            var error = new Error { Id = 4, Message = exception.Message };

            return new XmlResult(error);
        }

        public ActionResult General(Exception exception)
        {
            return Content("General failure", "text/plain");
        }
    }
}
