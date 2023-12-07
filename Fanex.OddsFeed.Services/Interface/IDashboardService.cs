namespace Fanex.OddsFeed.Services.Interface
{
    using Fanex.OddsFeed.Models.Report;

    public interface IDashboardService
    {
        Task<AccountStatistic> GetAccountStatisticsAsync(short systemId);
        
        Task<IEnumerable<LoginInfoStatistic>> GetLoginInfoAsync(int userId);
    }
}
