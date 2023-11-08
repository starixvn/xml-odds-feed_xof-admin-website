namespace Fanex.OddsFeed.Core.Service.Interface
{
    using Fanex.OddsFeed.Core.Model.Report;
    public interface IDashboardService
    {
        Task<AccountStatistic> GetAccountStatistics(short systemId);
        Task<IEnumerable<LoginInfoStatistic>> GetLoginInfo(int userId);
    }
}
