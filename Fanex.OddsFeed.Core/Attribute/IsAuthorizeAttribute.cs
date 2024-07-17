namespace Fanex.OddsFeed.Core.Attribute
{
    using System;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class IsAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var sessionService = new SessionService(context.HttpContext.Session);
            if (!sessionService.IsAuthorize)
            {
                if (IsAjaxRequest(context.HttpContext.Request))
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                }
                else
                {
                    context.Result = new RedirectToActionResult("index", "login", null);
                }
            }
        }

        private static bool IsAjaxRequest(HttpRequest request)
           => string.Equals(request.Query["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal)
              || string.Equals(request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal);
    }
}