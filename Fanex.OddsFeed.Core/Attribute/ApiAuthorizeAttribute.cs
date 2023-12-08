namespace Fanex.OddsFeed.Core.Attribute
{
    using System.Net;
    using Fanex.OddsFeed.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.DependencyInjection;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var apiKeyFromRequest = context.HttpContext.Request.Headers["ApiKey"];

            var appSettings = context.HttpContext.RequestServices.GetService<AppSettings>();
            var apiKeyFromConfig = appSettings?.ApiKey;

            if (string.IsNullOrEmpty(apiKeyFromRequest) || apiKeyFromRequest != apiKeyFromConfig)
            {
                context.Result = new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    Content = "API Key not found or incorrect",
                    ContentType = "text/html"
                };
            }
        }
    }
}
