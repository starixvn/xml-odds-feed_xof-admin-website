namespace Fanex.OddsFeed.Core.Attribute
{
    using System.Net;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Configuration;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            const string HeaderKeyName = "ApiKey";
            var apiKeyFromRequest = context.HttpContext.Request.Headers[HeaderKeyName];

            var serviceProvider = context.HttpContext.RequestServices;
            var configuration = (IConfiguration?)serviceProvider.GetService(typeof(IConfiguration));
            var apiKeyFromConfig = configuration?["AppSettings:ApiKey"];

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
