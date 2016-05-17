// --------------------------------------------------------------------------
// <copyright file="DomainController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.2.25">Created</change>
// </history>
namespace Fanex.OddsFeed.Admin.Controllers
{
    using Fanex.OddsFeed.Core.Criteria.Domain;
    using Fanex.OddsFeed.Core.Service.Impl;
    using Fanex.OddsFeed.Core.Service.Interface;
    using Fanex.OddsFeed.Core.Web;
    using Fanex.OddsFeed.Core.Web.Attribute;
    using System.Web.Mvc;

    /// <summary>
    /// Class DomainController.
    /// </summary>
    [IsAuthorize]
    public class DomainController : BaseController
    {
        #region Members

        /// <summary>
        /// The _domain service.
        /// </summary>
        private readonly IDomainService _domainService;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainController"/> class.
        /// </summary>
        public DomainController()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainController"/> class.
        /// </summary>
        /// <param name="domainService">The domain service.</param>
        public DomainController(IDomainService domainService = null)
        {
            _domainService = domainService ?? new DomainService { ServiceContext = ServiceContext };
        }

        #endregion

        public ActionResult ListDomain()
        {
            if (!this.IsAccountManagerRole())
            {
                return this.NoAuthorization();
            }

            ViewBag.listLicensees = _domainService.GetSelectList("Odds_SystemSel");
            return View();
        }

        public JsonResult GetAllDomain(GetDomainByPageCriteria criteria)
        {
            criteria.DomainName = criteria.DomainName == ViewResources.Domain_Name ? string.Empty : criteria.DomainName;
            criteria.PageIndex++;
            var dr = _domainService.GetAllDomain(criteria);
            var result = FanexGridHelper.DataForFanexGrid_PreventContainQuote(dr);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public string GetDomainSetting(GetDomainSettingByPageCriteria criteria)
        {
            criteria.PageIndex++;
            var dr = _domainService.GetDomainSetting(criteria);
            return FanexGridHelper.DataForFanexGrid(dr);
        }

        [HttpPost]
        public JsonResult CreateDomain(CreateDomainCriteria criteria)
        {
            criteria.Creator = CurrentSession.UserId;
            criteria.CurrentIP = CurrentContext.IP;
            var res = _domainService.CreateDomain(criteria);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateDomain(UpdateDomainCriteria criteria)
        {
            criteria.Creator = CurrentSession.UserId;
            criteria.CurrentIP = CurrentContext.IP;
            criteria.IP = string.IsNullOrEmpty(criteria.IP) ? string.Empty : criteria.IP;
            criteria.ISP = string.IsNullOrEmpty(criteria.ISP) ? string.Empty : criteria.ISP;
            var res = _domainService.UpdateDomain(criteria);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public int DeleteDomain(DeleteDomainCriteria criteria)
        {
            criteria.Creator = CurrentSession.UserId;
            criteria.Ip = CurrentContext.IP;
            var res = _domainService.DeleteDomain(criteria);
            return res;
        }

        public JsonResult DomainList_GetTop10(GetTop10DomainCriteria criteria)
        {
            var top10DomainList = this._domainService.GetDataSet(criteria).Tables[0];
            return JsonForAutoComplete(top10DomainList, "DomainID", "DomainName");
        }
    }
}
