namespace Fanex.OddsFeed.Services.Interface
{
    using Fanex.OddsFeed.Models.Report;
    public interface IDashboardService
    {
        Task<AccountStatistic> GetAccountStatistics(short systemId);
        Task<IEnumerable<LoginInfoStatistic>> GetLoginInfo(int userId);
    }
}
