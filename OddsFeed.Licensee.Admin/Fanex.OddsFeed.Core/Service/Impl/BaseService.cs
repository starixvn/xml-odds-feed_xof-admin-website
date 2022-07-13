namespace Fanex.OddsFeed.Core.Service.Impl
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.OddsFeed.Core.Helpers;
    using Fanex.OddsFeed.Core.Service.Interface;
    using Fanex.OddsFeed.Core.Repository.Interface;
    using Fanex.OddsFeed.Core.Repository.Impl;
    using Fanex.OddsFeed.Core.Model;

    public class BaseService : IBaseService
    {
        private readonly IBaseRepository baseRepository;
        private readonly IDynamicRepository dynamicRepository;

        public BaseService() : this(null, null)
        {
        }

        public BaseService(
            IDynamicRepository dynamicRepository,
            IBaseRepository baseRepository)
        {
            this.baseRepository = baseRepository ?? new BaseRepository();
            this.dynamicRepository = dynamicRepository ?? new DynamicRepository();
        }

        public DataCollection GetData(ICriteria criteria)
        {
            return baseRepository.ReturnDataCollection(criteria);
        }

        public IEnumerable<T> GetData<T>(ICriteria criteria)
        {
            if (criteria != null && criteria.IsValid())
            {
                return dynamicRepository.Fetch<T>(criteria);
            }

            return new List<T>();
        }

        public T GetDataEntity<T>(ICriteria criteria)
        {
            if (criteria != null && criteria.IsValid())
            {
                return dynamicRepository.Get<T>(criteria);
            }

            return default(T);
        }

        public CommandResult ExecuteNonQuery(ICriteria criteria)
        {
            return baseRepository.ExecuteNonQuery(criteria);
        }
    }
}