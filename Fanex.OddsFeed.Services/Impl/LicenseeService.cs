namespace Fanex.OddsFeed.Services.Impl
{
    using Fanex.OddsFeed.Criteria.Licensee;
    using Fanex.OddsFeed.Models;
    using Fanex.OddsFeed.Services.Interface;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class LicenseeService : ILicenseeService
    {
        private readonly IBaseService baseService;

        public LicenseeService(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        public async Task<bool> GetActiveBySystemIdAsync(int systemId)
        {
            var criteria = new GetActivateByLicenseeIDCriteria
            {
                SystemID = systemId
            };
            return await baseService.GetDataEntityAsync<bool>(criteria);
        }

        public async Task<PartnersProfile> GetProfileAllPartnersBySystemIdAsync(int systemId)
        {
            var criteria = new GetProfileAllPartnersBySystemIdCriteria
            {
                SystemID = systemId,
                AffsOddsType = string.Empty,
                AffsLanguageList = string.Empty,
                AffsSportIDList = string.Empty,
                AffsCountryCodeList = string.Empty,
                AffsLeagueIDList = string.Empty,
                AffsOutrightIDList = string.Empty,
                AffsBettypeIDList = string.Empty,
            };
            await baseService.GetDataEntityAsync<PartnersProfile>(criteria);

            return new PartnersProfile
            {
                AffsOddsType = criteria.AffsOddsType,
                AffsLanguageList = criteria.AffsLanguageList,
                AffsSportIDList = criteria.AffsSportIDList,
                AffsCountryCodeList = criteria.AffsCountryCodeList,
                AffsLeagueIDList = criteria.AffsLeagueIDList,
                AffsOutrightIDList = criteria.AffsOutrightIDList,
                AffsBettypeIDList = criteria.AffsBettypeIDList
            };
        }

        public IList<SelectListItem> GetResultDelayTimeList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "5", Text = "5" },
                new SelectListItem { Value = "10", Text = "10" },
                new SelectListItem { Value = "15", Text = "15" },
                new SelectListItem { Value = "20", Text = "20" },
                new SelectListItem { Value = "25", Text = "25" },
                new SelectListItem { Value = "30", Text = "30" }
            };
        }
    }
}
