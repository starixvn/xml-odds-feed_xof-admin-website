namespace Fanex.OddsFeed.Core.Service
{
    using System.Collections.Generic;
    using Fanex.OddsFeed.Core.Model.Report;

    public interface IDashboardService
    {
        AccountStatistic GetAccountStatistics(short systemId);

        IEnumerable<LoginInfoStatistic> GetLoginInfo(int userId);
    }
}