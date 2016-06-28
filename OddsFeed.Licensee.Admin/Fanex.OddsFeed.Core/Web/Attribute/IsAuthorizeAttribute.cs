// --------------------------------------------------------------------------
// <copyright file="IsAuthorizeAttribute.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.03.12">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Web.Attribute
{
    using System.Web.Mvc;
    using Fanex.OddsFeed.Core.Web.Session;

    /// <summary>
    /// Class IsAuthorizeAttribute.
    /// </summary>
    public class IsAuthorizeAttribute : ActionFilterAttribute
    {        
        private ICurrentSession _currentSession = new CurrentSession();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {            
            if (_currentSession.OnUserId == 0)
            {                
                filterContext.Result = new RedirectResult("~/login/index");                
            }            
        }
    }
}
