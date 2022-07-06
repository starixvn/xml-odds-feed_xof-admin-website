namespace Fanex.OddsFeed.Core.Service
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.OddsFeed.Core.Criteria.Dashboard;
    using Fanex.OddsFeed.Core.Helpers;
    using Fanex.OddsFeed.Core.Model.Report;

    public interface IDashboardService
    {
        AccountStatistic GetAccountStatistics(byte systemId);

        IEnumerable<LoginInfoStatistic> GetLoginInfo(int userId);

        DataCollection GetCurrentOnlineUserReturnDataCollection(GetCurrentOnlineUserCriteria criteria);

        DataCollection GetRecentActivitiesReturnDataCollection(GetRecentActivitiesCriteria criteria);
    }
}