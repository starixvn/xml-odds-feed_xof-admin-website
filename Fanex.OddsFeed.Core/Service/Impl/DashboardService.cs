using Fanex.OddsFeed.Core.Criteria.Dashboard;
using Fanex.OddsFeed.Core.Model.Report;
using Fanex.OddsFeed.Core.Service.Interface;

namespace Fanex.OddsFeed.Core.Service.Impl
{
    public class DashboardService : IDashboardService
    {
        private readonly IBaseService baseService;
        public DashboardService(IBaseService baseService) 
        {
            this.baseService = baseService;
        }

        public async Task<AccountStatistic> GetAccountStatistics(short systemId)
        {
            var criteria = new GetAccountStatisticsCriteria
            {
                SystemID = systemId
            };
            return await baseService.GetDataEntityAsync<AccountStatistic>(criteria);
        }

        public async Task<IEnumerable<LoginInfoStatistic>> GetLoginInfo(int userId)
        {
            var criteria = new GetLoginInfoCriteria
            {
                UserID = userId
            };
            return await baseService.GetDataAsync<LoginInfoStatistic>(criteria);
        }
    }
}
