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
    using Fanex.OddsFeed.Core;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Class IsUnderMaintenanceAttribute.
    /// </summary>    
    public class IsUnderMaintainanceAttribute : ActionFilterAttribute
    {
        public IsUnderMaintainanceAttribute()
        {
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string startTime = string.Empty;
            string endTime = string.Empty;

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
