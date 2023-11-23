namespace Fanex.OddsFeed.Controllers
{
    using Core;
    using Fanex.OddsFeed.Core.Attribute;
    using Fanex.OddsFeed.Core.Context;
    using Fanex.OddsFeed.Core.Session;
    using Fanex.OddsFeed.Criteria.Domain;
    using Fanex.OddsFeed.Models;
    using Fanex.OddsFeed.Models.ValueObject;
    using Fanex.OddsFeed.Resources;
    using Fanex.OddsFeed.Services.Interface;
    using Microsoft.AspNetCore.Mvc;

    [IsAuthorize]
    public class DomainController : BaseController
    {
        public DomainController(
            ICurrentContext currentContext, 
            ICurrentSession currentSession, 
            IBaseService baseService,
            AppSettings appSettings) 
            : base(currentContext, currentSession, baseService, appSettings)
        {
        }

        public IActionResult ListDomain()
        {
            if (!this.IsAccountManagerRole())
            {
                return this.NoAuthorization();
            }

            return View();
        }

        public async Task<IActionResult> GetAllDomain([FromBody] GetDomainByPageCriteria criteria)
        {
            criteria.DomainName = criteria.DomainName == ViewResources.Domain_Name ? string.Empty : criteria.DomainName;
            criteria.PageIndex++;
            var result = await baseService.GetDataAsync<Domain>(criteria);
            return new CustomJsonResult(new { data = result, totalItems = criteria.RowCount });
        }

        public async Task<IActionResult> GetDomainSetting([FromBody] GetDomainSettingByPageCriteria criteria)
        {
            criteria.PageIndex++;
            var result = await baseService.GetDataAsync<DomainSetting>(criteria);
            return new CustomJsonResult(new { data = result, totalItems = criteria.RowCount });
        }

        [HttpPost]
        public async Task<IActionResult> CreateDomain([FromBody] CreateDomainCriteria criteria)
        {
            criteria.Creator = currentSession.UserId;
            criteria.CurrentIP = currentContext.IP;
            var result = await baseService.ExecuteNonQueryAsync(criteria);
            return Json(result.Code);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDomain([FromBody] UpdateDomainCriteria criteria)
        {
            criteria.Creator = currentSession.UserId;
            criteria.CurrentIP = currentContext.IP;
            criteria.IP = string.IsNullOrEmpty(criteria.IP) ? string.Empty : criteria.IP;
            criteria.ISP = string.IsNullOrEmpty(criteria.ISP) ? string.Empty : criteria.ISP;
            var result = await baseService.ExecuteNonQueryAsync(criteria);
            return Json(result.Code);
        }

        public async Task<IActionResult> DeleteDomain([FromBody] DeleteDomainCriteria criteria)
        {
            criteria.Creator = currentSession.UserId;
            criteria.Ip = currentContext.IP;
            var result = await baseService.ExecuteNonQueryAsync(criteria);
            return Json(result.Code);
        }

        public async Task<IActionResult> DomainList_GetTop10([FromBody] GetTop10DomainCriteria criteria)
        {
            var domains =await baseService.GetDataAsync<Domain>(criteria);
            return Json(domains.Select(x => new JSONSource { Value = x.DomainId.ToString(), Label = x.DomainName }).ToList());
        }
    }
}
