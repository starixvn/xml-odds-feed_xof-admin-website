namespace Fanex.OddsFeed.Core.Service.Interface
{
    using System.Data;
    using Fanex.OddsFeed.Core.Criteria.Dashboard;
    using Fanex.OddsFeed.Core.Helpers;

    public interface IDashboardService
    {
        DataSet GetAccountStatisticsDataSet(GetAccountStatisticsCriteria criteria);

        DataSet GetLoginInfoReturnDataSet(GetLoginInfoCriteria criteria);

        DataCollection GetCurrentOnlineUserReturnDataCollection(GetCurrentOnlineUserCriteria criteria);

        DataCollection GetRecentActivitiesReturnDataCollection(GetRecentActivitiesCriteria criteria);        
    }
}
