namespace Fanex.OddsFeed.Controllers
{
    using System.Text;
    using Microsoft.AspNetCore.Mvc;
    using Core;
    using Fanex.OddsFeed.Core.Context;
    using Fanex.OddsFeed.Core.Session;
    using Fanex.OddsFeed.Core.Attribute;
    using Fanex.OddsFeed.Services.Interface;
    using Fanex.OddsFeed.Models;
    using Fanex.OddsFeed.Criteria.CompanyIP;

    [IsAuthorize]
    public class CompanyController : BaseController
    {
        public CompanyController(
            ICurrentContext currentContext, 
            ICurrentSession currentSession, 
            IBaseService baseService,
            AppSettings appSettings) : base(currentContext, currentSession, baseService, appSettings)
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
            var companyIPs =await baseService.GetDataAsync<CompanyIP>(new GetAllCompanyIPCriteria());
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
            criteria.Creator = currentSession.UserId;
            criteria.IP = currentContext.IP;
            CommandResult result =await baseService.ExecuteNonQueryAsync(criteria);
            return result.Code;
        }

        [HttpPost]
        public async Task<int> EditCompanyIPSetting([FromBody] UpdateCompanyIPCriteria criteria)
        {
            criteria.Creator = currentSession.UserId;
            criteria.Description = string.IsNullOrEmpty(criteria.Description) ? string.Empty : criteria.Description;
            CommandResult result = await baseService.ExecuteNonQueryAsync(criteria);
            return result.Code;
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCompanyIPSetting([FromBody] DeleteCompanyIPCriteria criteria)
        {
            criteria.Creator = currentSession.UserId;
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
