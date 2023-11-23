namespace Fanex.OddsFeed.Repositories.Interface
{
    using Fanex.Data.Repository;
    using Fanex.OddsFeed.Models;

    public interface ILicenseeRepository
    {
        Task<PartnersProfile> GetProfileAllPartnersBySystemId(ICriteria criteria);
    }
}
