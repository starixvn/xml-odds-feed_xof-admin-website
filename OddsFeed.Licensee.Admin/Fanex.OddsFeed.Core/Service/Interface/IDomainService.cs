namespace Fanex.OddsFeed.Core.Service.Interface
{
    using Fanex.OddsFeed.Core.Helpers;
    using Data.Repository;

    public interface IDomainService
    {
        DataCollection GetAllDomain(ICriteria criteria);

        DataCollection GetDomainSetting(ICriteria criteria);

        int CreateDomain(ICriteria criteria);

        int UpdateDomain(ICriteria criteria);

        int DeleteDomain(ICriteria criteria);
    }
}