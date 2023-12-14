namespace Fanex.OddsFeed.Core.Attribute
{
    using System;
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
                context.Result = new RedirectResult("~/login/LogOut");
            }
        }
    }
}