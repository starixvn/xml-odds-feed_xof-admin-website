// --------------------------------------------------------------------------
// <copyright file="AdministratorService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
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
    using System.Web.Mvc;
    using Fanex.Common.Helper;
    using Fanex.Databank.Repositories;
    using Fanex.Logging;
    using Fanex.OddsFeed.Core.Criteria.Administrator;
    using Fanex.OddsFeed.Core.Criteria.Misc;
    using Fanex.OddsFeed.Core.Domain;
    using Fanex.OddsFeed.Core.Helpers;
    using Fanex.OddsFeed.Core.Repository.Impl;
    using Fanex.OddsFeed.Core.Repository.Interface;
    using Fanex.OddsFeed.Core.Service.Interface;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class AdministratorService.
    /// </summary>    
    public class AdministratorService : BaseService, IAdministratorService
    {
        #region members        
        private readonly IAdministratorRepository _adminRepository;        
        private readonly SelectListRepository _selectListRepository;
        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AdministratorService"/> class.
        /// </summary>
        public AdministratorService()
        {
            _adminRepository = _adminRepository ?? new AdministratorRepository();
            _selectListRepository = _selectListRepository ?? new SelectListRepository();
        }

        #endregion

        #region methods

        /// <summary>
        /// Inserts the admin.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>ICommand Result.</returns>
        public ICommandResult InsertAdmin(InsertAdminCriteria criteria)
        {
            return _adminRepository.InsertAdmin(criteria);
        }

        /// <summary>
        /// Gets the admin list return data collection.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Data Collection.</returns>
        public DataCollection GetAdminListReturnDataCollection(GetAdminCriteria criteria)
        {
            try
            {
                if (criteria.IsValid())
                {
                    return _adminRepository.ReturnDataCollection(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Gets the admin list return data collection.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Data Collection.</returns>
        public DataCollection GetAdminListReturnDataCollection(CheckExistAdministratorCriteria criteria)
        {
            try
            {
                if (criteria.IsValid())
                {
                    return _adminRepository.ReturnDataCollection(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Gets the top10 admin return data set.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The data set.</returns>
        public DataSet GetTop10AdminReturnDataSet(GetTop10AdminCriteria criteria)
        {
            try
            {
                if (criteria.IsValid())
                {
                    return _adminRepository.ReturnDataSet(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Gets the existed admin data set.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The data set.</returns>
        public DataSet GetExistedAdminDataSet(GetExistedAdminCriteria criteria)
        {
            try
            {
                if (criteria.IsValid())
                {
                    return _adminRepository.ReturnDataSet(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Updates the admin.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>ICommand Result.</returns>
        public ICommandResult UpdateAdmin(CriteriaBase criteria)
        {
            // check parameters
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    return _adminRepository.Update(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Updates the admin information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>ICommand Result.</returns>
        public ICommandResult UpdateAdminInfo(UpdateAdminInfoCriteria criteria)
        {
            // check parameters
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    return _adminRepository.Update(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Updates the admin system.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>ICommand Result.</returns>
        public ICommandResult UpdateAdminSystem(UpdateAdminSystemCriteria criteria)
        {
            // check parameters
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    return _adminRepository.Update(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Updates the admin password.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>ICommand Result.</returns>
        public ICommandResult UpdateAdminPassword(UpdateAdminPasswordCriteria criteria)
        {
            // check parameters
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    return _adminRepository.Update(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }      

        /// <summary>
        /// Admins the list_ get top10.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The data set.</returns>
        public DataSet AdminList_GetTop10(AdminList_GetTop10Criteria criteria)
        {
            try
            {
                if (criteria.IsValid())
                {
                    return _adminRepository.ReturnDataSet(criteria);
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
