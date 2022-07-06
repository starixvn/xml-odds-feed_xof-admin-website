namespace Fanex.OddsFeed.Core.Repository.Interface
{
    using Fanex.OddsFeed.Core.Model;
    using Data.Repository;

    public interface ILicenseeRepository
    {
        PartnersProfile GetProfileAllPartnersBySystemId(ICriteria criteria);
    }
}