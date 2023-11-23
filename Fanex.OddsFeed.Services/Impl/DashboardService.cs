using Fanex.OddsFeed.Criteria.Dashboard;
using Fanex.OddsFeed.Models.Report;
using Fanex.OddsFeed.Services.Interface;

namespace Fanex.OddsFeed.Services.Impl
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
