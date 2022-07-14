namespace Fanex.OddsFeed.Admin.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.OddsFeed.Core.Criteria.Domain;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Model.ValueObject;
    using Fanex.OddsFeed.Core.Web;
    using Fanex.OddsFeed.Core.Web.Attribute;
    using Fanex.OddsFeed.Framework.Mvc;

    [IsAuthorize]
    public class DomainController : BaseController
    {
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
            var result = baseService.GetDataGrid<Domain>(criteria);
            return new CustomJsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetDomainSetting(GetDomainSettingByPageCriteria criteria)
        {
            criteria.PageIndex++;
            var result = baseService.GetDataGrid<DomainSetting>(criteria);
            return new CustomJsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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