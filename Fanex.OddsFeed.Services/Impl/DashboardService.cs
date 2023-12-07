namespace Fanex.OddsFeed.Services.Impl
{
    using Fanex.OddsFeed.Criteria.Dashboard;
    using Fanex.OddsFeed.Models.Report;
    using Fanex.OddsFeed.Services.Interface;
    
    public class DashboardService : IDashboardService
    {
        private readonly IBaseService baseService;
        public DashboardService(IBaseService baseService) 
        {
            this.baseService = baseService;
        }

        public async Task<AccountStatistic> GetAccountStatisticsAsync(short systemId)
        {
            var criteria = new GetAccountStatisticsCriteria
            {
                SystemID = systemId
            };
            return await baseService.GetDataEntityAsync<AccountStatistic>(criteria);
        }

        public async Task<IEnumerable<LoginInfoStatistic>> GetLoginInfoAsync(int userId)
        {
            var criteria = new GetLoginInfoCriteria
            {
                UserID = userId
            };
            return await baseService.GetDataAsync<LoginInfoStatistic>(criteria);
        }
    }
}
