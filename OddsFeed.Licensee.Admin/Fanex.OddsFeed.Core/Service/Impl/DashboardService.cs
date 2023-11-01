namespace Fanex.OddsFeed.Core.Service
{
    using System.Collections.Generic;
    using Fanex.OddsFeed.Core.Criteria.Dashboard;
    using Fanex.OddsFeed.Core.Model.Report;
    using Fanex.OddsFeed.Core.Service.Impl;
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

        public AccountStatistic GetAccountStatistics(short systemId)
        {
            var criteria = new GetAccountStatisticsCriteria
            {
                SystemID = systemId
            };
            return baseService.GetDataEntity<AccountStatistic>(criteria);
        }

        public IEnumerable<LoginInfoStatistic> GetLoginInfo(int userId)
        {
            var criteria = new GetLoginInfoCriteria
            {
                UserID = userId
            };
            return baseService.GetData<LoginInfoStatistic>(criteria);
        }
    }
}