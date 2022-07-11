namespace Fanex.OddsFeed.Admin.Controllers
{
    using System.Text;
    using System.Web.Mvc;
    using Fanex.OddsFeed.Core.Criteria.CompanyIP;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Web;
    using Fanex.OddsFeed.Core.Web.Attribute;

    [IsAuthorize]
    public class CompanyController : BaseController
    {
        [HttpGet]
        public ActionResult ListCompanyIP()
        {
            if (!this.IsAccountManagerRole())
            {
                return this.NoAuthorization();
            }

            var criteria = new InsertCompanyIPsCriteria();
            var companyIPs = baseService.GetData<CompanyIP>(new GetAllCompanyIPCriteria());
            var listIPFrom = new StringBuilder();
            var listIPTo = new StringBuilder();
            var listDescription = new StringBuilder();
            foreach (var record in companyIPs)
            {
                listIPFrom.AppendFormat("{0},", record.IPFrom);
                listIPTo.AppendFormat("{0},", record.IPTo);
                listDescription.AppendFormat("{0}\n", record.Description);
            }

            criteria.ListIPFrom = listIPFrom.ToString().Trim(',');
            criteria.ListIPTo = listIPTo.ToString().Trim(',');
            criteria.ListDescription = listDescription.ToString().Trim(',');

            return View(criteria);
        }

        public int EditCompanyIPs(InsertCompanyIPsCriteria criteria)
        {
            criteria.Creator = currentSession.UserId;
            criteria.IP = currentContext.IP;
            CommandResult result = baseService.ExecuteNonQuery(criteria);
            return result.Code;
        }

        [HttpPost]
        public int EditCompanyIPSetting(UpdateCompanyIPCriteria criteria)
        {
            criteria.Creator = currentSession.UserId;
            criteria.Description = string.IsNullOrEmpty(criteria.Description) ? string.Empty : criteria.Description;
            CommandResult result = baseService.ExecuteNonQuery(criteria);
            return result.Code;
        }

        [HttpPost]
        public JsonResult DeleteCompanyIPSetting(DeleteCompanyIPCriteria criteria)
        {
            criteria.Creator = currentSession.UserId;
            criteria.IP = currentContext.IP;
            criteria.IPFromOld = string.IsNullOrEmpty(criteria.IPFromOld) ? string.Empty : criteria.IPFromOld;
            criteria.IPToOld = string.IsNullOrEmpty(criteria.IPToOld) ? string.Empty : criteria.IPToOld;

            CommandResult result = baseService.ExecuteNonQuery(criteria);
            if (result.IsSuccessful)
            {
                result.Message = ViewResources.ActionResult_Success_DeleteCompanyIPSetting;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}