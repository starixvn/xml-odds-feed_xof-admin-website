namespace Fanex.OddsFeed.Core.Attribute
{
    using System;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.DependencyInjection;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class IsAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var serviceProvider = context.HttpContext.RequestServices;
            var sessionService = serviceProvider.GetService<ISessionService>();

            if (sessionService?.OnUserId == 0)
            {
                if (IsAjaxRequest(context.HttpContext.Request))
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                }
                else
                {
                    context.Result = new RedirectToActionResult("LogOut", "login", null);
                }
            }
        }

        private static bool IsAjaxRequest(HttpRequest request)
           => string.Equals(request.Query["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal) ||
              string.Equals(request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal);

    }
}