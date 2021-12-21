namespace Fanex.OddsFeed.Core.Web.Attribute
{
    using System;
    using System.Web.Mvc;
    using Fanex.OddsFeed.Core.Web.Session;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class IsAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly ICurrentSession _currentSession = new CurrentSession();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (_currentSession.OnUserId == 0)
            {
                filterContext.Result = new RedirectResult("~/login/LogOut");
            }
        }
    }
}