namespace Fanex.OddsFeed.Admin.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.OddsFeed.Core.Criteria.Domain;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Model.ValueObject;
    using Fanex.OddsFeed.Core.Service.Impl;
    using Fanex.OddsFeed.Core.Service.Interface;
    using Fanex.OddsFeed.Core.Web;
    using Fanex.OddsFeed.Core.Web.Attribute;

    [IsAuthorize]
    public class DomainController : BaseController
    {
        private readonly IDomainService _domainService;

        public DomainController() : this(null)
        {
        }

        public DomainController(IDomainService domainService)
        {
            _domainService = domainService ?? new DomainService();
        }

        public ActionResult ListDomain()
        {
            if (!this.IsAccountManagerRole())
            {
                return this.NoAuthorization();
            }

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
            criteria.Creator = currentSession.UserId;
            criteria.CurrentIP = currentContext.IP;
            CommandResult result = baseService.ExecuteNonQuery(criteria);
            return Json(result.Code, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateDomain(UpdateDomainCriteria criteria)
        {
            criteria.Creator = currentSession.UserId;
            criteria.CurrentIP = currentContext.IP;
            criteria.IP = string.IsNullOrEmpty(criteria.IP) ? string.Empty : criteria.IP;
            criteria.ISP = string.IsNullOrEmpty(criteria.ISP) ? string.Empty : criteria.ISP;
            CommandResult result = baseService.ExecuteNonQuery(criteria);
            return Json(result.Code, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteDomain(DeleteDomainCriteria criteria)
        {
            criteria.Creator = currentSession.UserId;
            criteria.Ip = currentContext.IP;
            CommandResult result = baseService.ExecuteNonQuery(criteria);
            return Json(result.Code, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DomainList_GetTop10(GetTop10DomainCriteria criteria)
        {
            var domains = baseService.GetData<Domain>(criteria);
            return Json(domains.Select(x => new JSONSource { Value = x.DomainId.ToString(), Label = x.DomainName }).ToList());
        }
    }
}