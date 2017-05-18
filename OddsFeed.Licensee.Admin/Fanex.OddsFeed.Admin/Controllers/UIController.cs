// --------------------------------------------------------------------------
// <copyright file="UIController.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.02.05">Create</change>
// </history>
namespace Fanex.OddsFeed.Admin.Controllers
{
    using Fanex.Caching;
    using Fanex.OddsFeed.Core.Criteria.Administrator;
    using Fanex.OddsFeed.Core.Service.Impl;
    using System.Collections.Generic;
    using System.Web.Mvc;
    /// <summary>
    /// Class UIController.
    /// </summary>
    ////[Authorize]
    public class UIController : BaseController
    {
        /// <summary>
        /// The cache key sitemap data.
        /// </summary>
        private const string CacheKeySitemapData = "UIController.CacheKeySitemapData";

        /// <summary>
        /// The _cache manager.
        /// </summary>
        private readonly ICache _cacheManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UIController"/> class.
        /// </summary>
        public UIController() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIController"/> class.
        /// </summary>
        /// <param name="cacheManager">The cache manager.</param>
        public UIController(ICache cacheManager)
        {
            _cacheManager = cacheManager ?? CacheService.CreateInstance();
        }

        ////[ChildActionOnly]

        public ActionResult Header()
        {
            ////return PartialView(this.CurrentUser);
            ViewBag.SystemList = GenerateSystemList();
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult GlobalScript()
        {
            ////ViewBag.Employee = this.CurrentUser;
            return PartialView();
        }

        /// <summary>
        /// Navigate the bar.
        /// </summary>
        /// <returns>Partial View Result.</returns>
        [ChildActionOnly]
        public PartialViewResult NavBar()
        {
            var routeData = ControllerContext.ParentActionViewContext.RouteData;
            ViewBag.Controller = routeData.Values["controller"];
            ViewBag.Action = routeData.Values["action"];
            ViewBag.RoleID = CurrentSession.UserRole;
            ////ViewBag.NavigationItems = GetNagivationData();

            CurrentSession.ControllerName = ViewBag.Controller;
            CurrentSession.ActionName = ViewBag.Action;

            return PartialView();
        }

        /// <summary>
        /// Generates the system list.
        /// </summary>
        /// <returns>IList {SelectListItem}.</returns>
        private IList<SelectListItem> GenerateSystemList()
        {
            var allSystemList = new AdministratorService().GetSelectList(new GetAllSystemsByUserIdCriteria
            {
                UserRole = CurrentSession.UserRole,
                UserId = CurrentSession.UserId
            });

            var strSystemList = string.Empty;
            foreach (var system in allSystemList)
            {
                strSystemList += system.Value + "," + system.Text + ";";
            }

            CurrentSession.SystemList = strSystemList;
            var systemList = new List<SelectListItem>();
            var systems = strSystemList.Split(';');
            foreach (string system in systems)
            {
                if (!string.IsNullOrEmpty(system))
                {
                    var nameId = system.Split(',');

                    var item = new SelectListItem();
                    item.Value = nameId[0] ?? string.Empty;
                    item.Text = nameId[1] ?? string.Empty;
                    item.Selected = item.Value == CurrentSession.SystemId.ToString();

                    systemList.Add(item);
                }
            }

            return systemList;
        }
    }
}