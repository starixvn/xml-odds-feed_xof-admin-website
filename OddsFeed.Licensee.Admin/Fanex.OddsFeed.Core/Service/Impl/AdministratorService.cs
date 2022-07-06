namespace Fanex.OddsFeed.Core.Service.Impl
{
    using System.Data;
    using Data.Repository;
    using Fanex.Common.Helper;
    using Fanex.OddsFeed.Core.Criteria.Administrator;
    using Fanex.OddsFeed.Core.Helpers;
    using Fanex.OddsFeed.Core.Service.Interface;
    using Fanex.OddsFeed.Framework.Data;

    public class AdministratorService : IAdministratorService
    {
        private readonly IBaseService baseService;

        public AdministratorService() : this(null)
        {
        }

        public AdministratorService(IBaseService baseService)
        {
            this.baseService = baseService ?? new BaseService();
        }

        public ICommandResult InsertAdmin(InsertAdminCriteria criteria)
        {
            return baseService.ExecuteNonQuery(criteria);
        }

        public DataCollection GetAdminListReturnDataCollection(GetAdminCriteria criteria)
        {
            if (criteria.IsValid())
            {
                return baseService.GetData(criteria);
            }

            return null;
        }

        public DataCollection GetAdminListReturnDataCollection(CheckExistAdministratorCriteria criteria)
        {
            if (criteria.IsValid())
            {
                return baseService.GetData(criteria);
            }

            return null;
        }

        public DataSet GetTop10AdminReturnDataSet(GetTop10AdminCriteria criteria)
        {
            if (criteria.IsValid())
            {
                return baseService.GetDataSet(criteria);
            }

            return null;
        }

        public DataSet GetExistedAdminDataSet(GetExistedAdminCriteria criteria)
        {
            if (criteria.IsValid())
            {
                return baseService.GetDataSet(criteria);
            }

            return null;
        }

        public ICommandResult UpdateAdmin(CriteriaBase criteria)
        {
            // check parameters
            Check.Argument.IsNotNull(criteria, "criteria");

            if (criteria.IsValid())
            {
                return baseService.ExecuteNonQuery(criteria);
            }

            return null;
        }

        public ICommandResult UpdateAdminInfo(UpdateAdminInfoCriteria criteria)
        {
            // check parameters
            Check.Argument.IsNotNull(criteria, "criteria");

            if (criteria.IsValid())
            {
                return baseService.ExecuteNonQuery(criteria);
            }

            return null;
        }

        public ICommandResult UpdateAdminSystem(UpdateAdminSystemCriteria criteria)
        {
            // check parameters
            Check.Argument.IsNotNull(criteria, "criteria");

            if (criteria.IsValid())
            {
                return baseService.ExecuteNonQuery(criteria);
            }

            return null;
        }

        public ICommandResult UpdateAdminPassword(UpdateAdminPasswordCriteria criteria)
        {
            // check parameters
            Check.Argument.IsNotNull(criteria, "criteria");

            if (criteria.IsValid())
            {
                return baseService.ExecuteNonQuery(criteria);
            }

            return null;
        }

        public DataSet AdminList_GetTop10(AdminList_GetTop10Criteria criteria)
        {
            if (criteria.IsValid())
            {
                return baseService.GetDataSet(criteria);
            }

            return null;
        }
    }
}