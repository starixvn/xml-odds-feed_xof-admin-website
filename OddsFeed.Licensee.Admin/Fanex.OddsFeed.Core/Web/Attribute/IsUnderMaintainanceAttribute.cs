// --------------------------------------------------------------------------
// <copyright file="IsUnderMaintainanceAttribute.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2013.03.21">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Web.Attribute
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public class IsUnderMaintainanceAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var startTime = string.Empty;
            var endTime = string.Empty;

            if (Utilities.GetDownTime(out startTime, out endTime))
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
                new
                {
                    action = "UnderMaintainance",
                    controller = "Tolerance",
                    timeFrom = startTime,
                    timeTo = endTime
                }));
            }
        }
    }
}