// --------------------------------------------------------------------------
// <copyright file="DashboardService.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2013.03.05">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Service.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;    
    using Fanex.Caching;
    using Fanex.Common.Helper;
    using Fanex.Databank.Repositories;
    using Fanex.Logging;
    using Fanex.OddsFeed.Core.Business;
    using Fanex.OddsFeed.Core.Criteria.Dashboard;
    using Fanex.OddsFeed.Core.Criteria.Misc;    
    using Fanex.OddsFeed.Core.Domain;
    using Fanex.OddsFeed.Core.Helpers;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Model.ValueObject;
    using Fanex.OddsFeed.Core.Repository.Impl;
    using Fanex.OddsFeed.Core.Repository.Interface;
    using Fanex.OddsFeed.Core.Service.Interface;    
    using Fanex.OddsFeed.Framework.Data;

    /// <summary>
    /// Class DashboardService.
    /// </summary>    
    public class DashboardService : BaseService, IDashboardService
    {
        #region members

        private readonly IBaseRepository _dashboardRepository;
        private readonly SelectListRepository _selectListRepository;

        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardService"/> class.
        /// </summary>
        public DashboardService()
        {
            _dashboardRepository = _dashboardRepository ?? new AdministratorRepository();
            _selectListRepository = _selectListRepository ?? new SelectListRepository();
        }

        #endregion

        #region methods

        /// <summary>
        /// Gets the account statistics data set.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Object DataSet.</returns>
        public DataSet GetAccountStatisticsDataSet(GetAccountStatisticsCriteria criteria)
        {
            try
            {
                if (criteria.IsValid())
                {
                    return _dashboardRepository.ReturnDataSet(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Gets the login information return data set.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Object DataSet.</returns>
        public DataSet GetLoginInfoReturnDataSet(GetLoginInfoCriteria criteria)
        {
            try
            {
                if (criteria.IsValid())
                {
                    return _dashboardRepository.ReturnDataSet(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Gets the current online user return data collection.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Object DataCollection.</returns>
        public DataCollection GetCurrentOnlineUserReturnDataCollection(GetCurrentOnlineUserCriteria criteria)
        {
            try
            {
                if (criteria.IsValid())
                {
                    return _dashboardRepository.ReturnDataCollection(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Gets the recent activities return data collection.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Object DataCollection.</returns>
        public DataCollection GetRecentActivitiesReturnDataCollection(GetRecentActivitiesCriteria criteria)
        {
            try
            {
                if (criteria.IsValid())
                {
                    return _dashboardRepository.ReturnDataCollection(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        #endregion
    }
}
