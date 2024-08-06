namespace Fanex.OddsFeed.Controllers
{
    using System.Text;
    using System.Threading.Tasks;
    using Fanex.OddsFeed.Core;
    using Fanex.OddsFeed.Core.Attribute;
    using Fanex.OddsFeed.Core.Context;
    using Fanex.OddsFeed.Criteria.CompanyIP;
    using Fanex.OddsFeed.Models;
    using Fanex.OddsFeed.Resources;
    using Fanex.OddsFeed.Services;
    using Microsoft.AspNetCore.Mvc;

    [IsAuthorize]
    public class CompanyController : BaseController
    {
        public CompanyController(
            ICurrentContext currentContext, 
            ISessionService sessionService, 
            IBaseService baseService,
            AppSettings appSettings) : base(currentContext, sessionService, baseService, appSettings)
        {
        }

        [HttpGet]
        public async Task<IActionResult> ListCompanyIP()
        {
            if (!this.IsAccountManagerRole())
            {
                return this.NoAuthorization();
            }

            var criteria = new InsertCompanyIPsCriteria();
            var companyIPs = await baseService.GetDataAsync<CompanyIP>(new GetAllCompanyIPCriteria());
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

        public async Task<int> EditCompanyIPs([FromBody] InsertCompanyIPsCriteria criteria)
        {
            criteria.Creator = sessionService.UserId;
            criteria.IP = currentContext.IP;
            CommandResult result =await baseService.ExecuteNonQueryAsync(criteria);
            return result.Code;
        }

        [HttpPost]
        public async Task<int> EditCompanyIPSetting([FromBody] UpdateCompanyIPCriteria criteria)
        {
            criteria.Creator = sessionService.UserId;
            criteria.Description = string.IsNullOrEmpty(criteria.Description) ? string.Empty : criteria.Description;
            CommandResult result = await baseService.ExecuteNonQueryAsync(criteria);
            return result.Code;
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCompanyIPSetting([FromBody] DeleteCompanyIPCriteria criteria)
        {
            criteria.Creator = sessionService.UserId;
            criteria.IP = currentContext.IP;
            criteria.IPFromOld = string.IsNullOrEmpty(criteria.IPFromOld) ? string.Empty : criteria.IPFromOld;
            criteria.IPToOld = string.IsNullOrEmpty(criteria.IPToOld) ? string.Empty : criteria.IPToOld;

            CommandResult result = await baseService.ExecuteNonQueryAsync(criteria);
            if (result.IsSuccessful)
            {
                result.Message = ViewResources.ActionResult_Success_DeleteCompanyIPSetting;
            }

            return Json(result);
        }
    }
}
