// --------------------------------------------------------------------------
// <copyright file="SubAccService.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.2">Created</change>
// </history>
namespace Fanex.OddsFeed.Core.Service.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Common.Helper;
    using Fanex.Databank.Repositories;
    using Fanex.OddsFeed.Core.Criteria.Administrator;
    using Fanex.OddsFeed.Core.Criteria.Affiliate;
    using Fanex.OddsFeed.Core.Criteria.Misc;
    using Fanex.OddsFeed.Core.Criteria.SubAccount;
    using Fanex.OddsFeed.Core.Criteria.User;
    using Fanex.OddsFeed.Core.Domain;
    using Fanex.OddsFeed.Core.Helpers;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Repository.Impl;
    using Fanex.OddsFeed.Core.Repository.Interface;
    using Fanex.OddsFeed.Core.Service.Interface;
    using Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class SubACCService.
    /// </summary>    
    public class SubAccService : BaseService, ISubAccService
    {
        #region Members        
        private readonly IUserRepository _userRepository;        
        private readonly ISubAccRepository _subAccRepository;        
        private readonly SelectListRepository _selectListRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SubAccService"/> class.
        /// </summary>
        public SubAccService()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubAccService"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="subAccRepository">The sub account repository.</param>
        public SubAccService(IUserRepository userRepository, ISubAccRepository subAccRepository = null)            
        {
            _userRepository = userRepository ?? new UserRepository();
            _subAccRepository = subAccRepository ?? new SubAccRepository();            
            this._selectListRepository = this._selectListRepository ?? new SelectListRepository();
        }

        #endregion

        #region Get

        /// <summary>
        /// Gets the by name and system identifier.
        /// </summary>
        /// <param name="systemId">The system identifier.</param>
        /// <param name="name">The system name.</param>
        /// <returns>Object IList{User}.</returns>
        public IList<User> GetByNameAndSystemId(byte systemId, string name)
        {
            try
            {
                var criteria = new GetSubAccByNameCriteria
                {
                    SystemId = systemId,
                    RoleId = 3,
                    Username = name
                };

                if (criteria.IsValid())
                {
                    IEnumerable<IUser> siteList = _subAccRepository.Fetch(criteria).OrderBy(s => s.Name);

                    if (siteList != null)
                    {
                        return siteList.Select(s => new User
                        {
                            Id = s.Id,
                            Username = s.Name,
                            FirstName = s.FirstName,
                            LastName = s.LastName,
                            Email = s.Email,
                            Creator = s.Creator,
                            CreatedDate = Convert.ToDateTime(s.CreatedDate),
                            IsActive = s.IsActive
                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Gets the sub account list return data collection.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Object DataCollection.</returns>
        public DataCollection GetSubAccountListReturnDataCollection(GetSubAccByNameCriteria criteria)
        {
            try
            {
                if (criteria.IsValid())
                {
                    return _subAccRepository.ReturnDataCollection(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Gets the sub account list return data collection.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Object DataCollection.</returns>
        public DataCollection GetSubAccountListReturnDataCollection(CheckExistSubAccountCriteria criteria)
        {
            try
            {
                if (criteria.IsValid())
                {
                    return _subAccRepository.ReturnDataCollection(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Gets the top10 sub account return data set.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Object DataSet.</returns>
        public DataSet GetTop10SubAccountReturnDataSet(GetTop10SubAccCriteria criteria)
        {
            try
            {
                // check parameters
                Check.Argument.IsNotNull(criteria, "criteria");

                if (criteria.IsValid())
                {
                    return _subAccRepository.ReturnDataSet(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Gets the by system identifier.
        /// </summary>
        /// <param name="systemId">The system identifier.</param>
        /// <returns>Object IList{User}.</returns>
        public IList<User> GetBySystemId(byte systemId)
        {
            try
            {
                var criteria = new GetSubAccBySystemIDCriteria
                {
                    SystemId = systemId,
                    RoleId = 3
                };

                if (criteria.IsValid())
                {
                    IEnumerable<IUser> subAccList = _subAccRepository.Fetch(criteria).OrderBy(s => s.Name);

                    if (subAccList != null)
                    {
                        return subAccList.Select(s => new User
                        {
                            Id = s.Id,
                            Username = s.Name,
                            FirstName = s.FirstName,
                            LastName = s.LastName,
                            Email = s.Email,
                            Creator = s.Creator,
                            CreatedDate = Convert.ToDateTime(s.CreatedDate),
                            IsActive = s.IsActive
                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Gets the user permission.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="systemId">The system identifier.</param>
        /// <returns>Object PermissionFlag.</returns>
        public PermissionFlag GetUserPermission(int userId, byte systemId)
        {
            try
            {
                var criteria = new UserGetPermissionCriteria
                {
                    UserID = userId,
                    SystemID = systemId
                };

                if (criteria.IsValid())
                {
                    return _userRepository.GetPermissionFlags(ref criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }       

        #endregion

        #region Insert/Update/Delete

        /// <summary>
        /// Inserts the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Object ICommandResult.</returns>
        public ICommandResult Insert(InsertSubAccCriteria criteria)
        {
            // check parameters
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    return _subAccRepository.Insert(ref criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Updates the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Object ICommandResult.</returns>
        public ICommandResult Update(CriteriaBase criteria)
        {
            // check parameters
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    return _subAccRepository.Update(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Deletes the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Object ICommandResult.</returns>
        public ICommandResult Delete(CriteriaBase criteria)
        {
            // check parameters
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    return _subAccRepository.Delete(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Assigns the licensee.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Object ICommandResult.</returns>
        public ICommandResult AssignLicensee(AssignLicensee_SubAccCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            try
            {
                if (criteria.IsValid())
                {
                    return this._subAccRepository.Update(criteria);
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
