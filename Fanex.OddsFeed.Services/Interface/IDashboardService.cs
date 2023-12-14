namespace Fanex.OddsFeed.Services.Interface
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
