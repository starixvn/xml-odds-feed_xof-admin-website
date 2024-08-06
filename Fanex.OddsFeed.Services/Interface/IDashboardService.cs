namespace Fanex.OddsFeed.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.OddsFeed.Models.Report;

    public interface IDashboardService
    {
        Task<AccountStatistic> GetAccountStatisticsAsync(short systemId);
        
        Task<IEnumerable<LoginInfoStatistic>> GetLoginInfoAsync(int userId);
    }
}
