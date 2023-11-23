namespace Fanex.OddsFeed.Services.Interface
{
    using Fanex.OddsFeed.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;
    public interface ILicenseeService
    {
        Task<bool> GetActiveBySystemId(int systemId);

        Task<PartnersProfile> GetProfileAllPartnersBySystemId(int systemId);

        Task<IEnumerable<SelectListItem>> GetResultDelayTimeList();
    }
}
