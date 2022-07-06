namespace Fanex.OddsFeed.Core.Service.Impl
{
    using Fanex.OddsFeed.Core.Helpers;
    using Fanex.OddsFeed.Core.Repository.Impl;
    using Fanex.OddsFeed.Core.Repository.Interface;
    using Fanex.OddsFeed.Core.Service.Interface;
    using Data.Repository;

    public class DomainService : IDomainService
    {
        private readonly IDomainRepository domainRepository;

        public DomainService() : this(null)
        {
        }

        public DomainService(IDomainRepository domainRepository)
        {
            this.domainRepository = domainRepository ?? new DomainRepository();
        }

        public DataCollection GetAllDomain(ICriteria criteria)
        {
            return domainRepository.GetAllDomain(criteria);
        }

        public DataCollection GetDomainSetting(ICriteria criteria)
        {
            return domainRepository.GetDomainSetting(criteria);
        }

        public int CreateDomain(ICriteria criteria)
        {
            return domainRepository.CreateDomain(criteria);
        }

        public int UpdateDomain(ICriteria criteria)
        {
            return domainRepository.UpdateDomain(criteria);
        }

        public int DeleteDomain(ICriteria criteria)
        {
            return domainRepository.DeleteDomain(criteria);
        }
    }
}