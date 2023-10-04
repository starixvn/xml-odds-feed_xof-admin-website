namespace Fanex.OddsFeed.Core.Web.Attribute
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.Method)]
    public class IsAuthorizedAPIKeyAttribute : ActionFilterAttribute
    {
        private string ApiKeyXOF = ConfigurationManager.AppSettings["ApiKey"];
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            const string HeaderKeyName = "ApiKey";
            var apiKey = filterContext.HttpContext.Request.Headers.GetValues(HeaderKeyName);

            if (apiKey == null || !IsValidApiKey(apiKey.FirstOrDefault()))
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        private bool IsValidApiKey(string apiKey)
        {
            return apiKey == ApiKeyXOF;
        }
    }
}
