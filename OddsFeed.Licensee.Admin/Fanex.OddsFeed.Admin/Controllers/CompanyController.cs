namespace Fanex.OddsFeed.Admin.Controllers
{
    using System.Data;
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
            var companyIPs = baseService.GetDataSet(new GetAllCompanyIPCriteria());
            var listIPFrom = new StringBuilder();
            var listIPTo = new StringBuilder();
            var listDescription = new StringBuilder();
            foreach (DataRow row in companyIPs.Tables[0].Rows)
            {
                listIPFrom.AppendFormat("{0},", row["IPFrom"].ToString());
                listIPTo.AppendFormat("{0},", row["IPTo"].ToString());
                listDescription.AppendFormat("{0}\n", row["Description"].ToString());
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
            var res = baseService.ExecuteNonQuery(criteria);
            return (int)res.Code;
        }

        [HttpPost]
        public int EditCompanyIPSetting(UpdateCompanyIPCriteria criteria)
        {
            criteria.Creator = currentSession.UserId;
            criteria.Description = string.IsNullOrEmpty(criteria.Description) ? string.Empty : criteria.Description;
            var result = baseService.ExecuteNonQuery(criteria);
            return (int)result.Code;
        }

        [HttpPost]
        public JsonResult DeleteCompanyIPSetting(DeleteCompanyIPCriteria criteria)
        {
            var deleteResult = new OddsFeedActionResult();
            criteria.Creator = currentSession.UserId;
            criteria.IP = currentContext.IP;
            criteria.IPFromOld = string.IsNullOrEmpty(criteria.IPFromOld) ? string.Empty : criteria.IPFromOld;
            criteria.IPToOld = string.IsNullOrEmpty(criteria.IPToOld) ? string.Empty : criteria.IPToOld;

            var result = baseService.ExecuteNonQuery(criteria);
            if (result.Code > 0)
            {
                deleteResult.IsSuccess = false;
                deleteResult.ErrorCode = result.Code;
                deleteResult.ResultMessage = result.Message;
            }
            else
            {
                deleteResult.IsSuccess = true;
                deleteResult.ResultMessage = ViewResources.ActionResult_Success_DeleteCompanyIPSetting;
            }

            return Json(deleteResult, JsonRequestBehavior.AllowGet);
        }
    }
}