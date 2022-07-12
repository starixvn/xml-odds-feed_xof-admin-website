namespace Fanex.OddsFeed.Core.Service.Impl
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Data.Repository;
    using Fanex.OddsFeed.Core.Helpers;
    using Fanex.OddsFeed.Core.Service.Interface;
    using Fanex.OddsFeed.Core.Repository;
    using Fanex.OddsFeed.Core.Repository.Interface;
    using Fanex.OddsFeed.Core.Repository.Impl;
    using Fanex.OddsFeed.Core.Model;

    public class BaseService : IBaseService
    {
        private readonly IBaseRepository baseRepository;
        private readonly IDynamicRepository dynamicRepository;
        private readonly ISelectListRepository selectListRepository;

        public BaseService() : this(null, null, null)
        {
        }

        public BaseService(
            IDynamicRepository dynamicRepository,
            ISelectListRepository selectListRepository,
            IBaseRepository baseRepository)
        {
            this.baseRepository = baseRepository ?? new BaseRepository();
            this.dynamicRepository = dynamicRepository ?? new DynamicRepository();
            this.selectListRepository = selectListRepository ?? new SelectListRepository();
        }

        public IEnumerable<SelectListItem> GetSelectList(ICriteria criteria, bool order = true, string selectedValue = "")
        {
            return selectListRepository.Get(criteria, order, selectedValue);
        }

        public IEnumerable<SelectListItem> GetSelectListWithGroup(ICriteria criteria, bool order = true, string selectedValue = "")
        {
            return selectListRepository.GetWithGroup(criteria, order, selectedValue);
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