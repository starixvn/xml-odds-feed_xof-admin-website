namespace Fanex.OddsFeed.Core.Service.Interface
{
    using Fanex.OddsFeed.Core.Model;
    using Microsoft.AspNetCore.Mvc.Rendering;
    public interface ILicenseeService
    {
        Task<bool> GetActiveBySystemId(int systemId);

        Task<PartnersProfile> GetProfileAllPartnersBySystemId(int systemId);

        Task<IEnumerable<SelectListItem>> GetResultDelayTimeList();
    }
}
