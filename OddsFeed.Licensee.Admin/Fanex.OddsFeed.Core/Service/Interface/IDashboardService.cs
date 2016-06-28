// --------------------------------------------------------------------------
// <copyright file="IDashboardService.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2013.03.05">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Service.Interface
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Fanex.OddsFeed.Core.Criteria.Dashboard;
    using Fanex.OddsFeed.Core.Domain;        
    using Fanex.OddsFeed.Core.Helpers;
    using Fanex.OddsFeed.Framework.Data;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed")]
    public interface IDashboardService : IBaseService
    {
        DataSet GetAccountStatisticsDataSet(GetAccountStatisticsCriteria criteria);

        DataSet GetLoginInfoReturnDataSet(GetLoginInfoCriteria criteria);

        DataCollection GetCurrentOnlineUserReturnDataCollection(GetCurrentOnlineUserCriteria criteria);

        DataCollection GetRecentActivitiesReturnDataCollection(GetRecentActivitiesCriteria criteria);        
    }
}
