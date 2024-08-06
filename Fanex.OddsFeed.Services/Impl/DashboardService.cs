namespace Fanex.OddsFeed.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.OddsFeed.Criteria.Dashboard;
    using Fanex.OddsFeed.Models.Report;
    
    public class DashboardService(IBaseService baseService) : IDashboardService
    {
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
