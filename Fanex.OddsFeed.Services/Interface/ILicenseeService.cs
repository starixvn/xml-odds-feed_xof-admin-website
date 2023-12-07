namespace Fanex.OddsFeed.Services.Interface
{
    using Fanex.OddsFeed.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface ILicenseeService
    {
        Task<bool> GetActiveBySystemIdAsync(int systemId);

        Task<PartnersProfile> GetProfileAllPartnersBySystemIdAsync(int systemId);

        IList<SelectListItem> GetResultDelayTimeList();
    }
}
