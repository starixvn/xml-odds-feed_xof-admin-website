namespace Fanex.OddsFeed.Core.Service.Impl
{
    using System.Data;
    using Fanex.OddsFeed.Core.Criteria.Dashboard;
    using Fanex.OddsFeed.Core.Helpers;
    using Fanex.OddsFeed.Core.Service.Interface;

    public class DashboardService : IDashboardService
    {
        private readonly IBaseService baseService;

        public DashboardService() : this(null)
        {
        }

        public DashboardService(IBaseService baseService)
        {
            this.baseService = baseService ?? new BaseService();
        }

        public DataSet GetAccountStatisticsDataSet(GetAccountStatisticsCriteria criteria)
        {
            if (criteria.IsValid())
            {
                return baseService.GetDataSet(criteria);
            }

            return null;
        }

        public DataSet GetLoginInfoReturnDataSet(GetLoginInfoCriteria criteria)
        {
            if (criteria.IsValid())
            {
                return baseService.GetDataSet(criteria);
            }

            return null;
        }

        public DataCollection GetCurrentOnlineUserReturnDataCollection(GetCurrentOnlineUserCriteria criteria)
        {
            if (criteria.IsValid())
            {
                return baseService.GetData(criteria);
            }

            return null;
        }

        public DataCollection GetRecentActivitiesReturnDataCollection(GetRecentActivitiesCriteria criteria)
        {
            if (criteria.IsValid())
            {
                return baseService.GetData(criteria);
            }

            return null;
        }
    }
}