namespace Fanex.OddsFeed.Core.Web.Attribute
{
    using System;
    using Fanex.OddsFeed.Core.Web.Session;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.DependencyInjection;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class IsAuthorizeAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var serviceProvider = context.HttpContext.RequestServices;
            var currentSession = serviceProvider.GetService<ICurrentSession>();

            if (currentSession?.OnUserId == 0)
            {
                context.Result = new RedirectResult("~/login/LogOut");
            }
        }
    }
}