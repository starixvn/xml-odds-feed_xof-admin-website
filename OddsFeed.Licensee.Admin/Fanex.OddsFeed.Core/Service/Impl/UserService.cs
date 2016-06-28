// --------------------------------------------------------------------------
// <copyright file="UserService.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.2">Created</change>
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
    using Fanex.OddsFeed.Core.Criteria.User;
    using Fanex.OddsFeed.Core.Domain;
    using Fanex.OddsFeed.Core.Helpers;
    using Fanex.OddsFeed.Core.Model;    
    using Fanex.OddsFeed.Core.Repository.Impl;
    using Fanex.OddsFeed.Core.Repository.Interface;
    using Fanex.OddsFeed.Core.Service.Interface;
    using Fanex.OddsFeed.Framework.Data;

    /// <summary>
    /// Class UserService.
    /// </summary>    
    public class UserService : BaseService, IUserService
    {        
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        public UserService() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public UserService(IUserRepository repository = null)
        {
            _userRepository = repository ?? new UserRepository();            
        }

        /// <summary>
        /// Users the login.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Object IUser.</returns>
        public IUser UserLogin(ref UserLoginUserCriteria criteria)
        {
            Check.Argument.IsNotEmpty(criteria.UserName, "username");
            Check.Argument.IsNotEmpty(criteria.UserPwd, "password");
            IUser user = null;

            try
            {
                if (criteria.IsValid())
                {
                    user = _userRepository.UserLogin(ref criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return user;
        }

        /// <summary>
        /// Users the get profile.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Object PermissionFlag.</returns>
        public PermissionFlag UserGetProfile(ref UserGetProfileCriteria criteria)
        {
            PermissionFlag flags = null;
            try
            {
                if (criteria.IsValid())
                {
                    flags = _userRepository.GetProfile(ref criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return flags;
        }

        /// <summary>
        /// Gets the permission flags.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Object PermissionFlag.</returns>
        public PermissionFlag GetPermissionFlags(ref UserGetPermissionCriteria criteria)
        {
            PermissionFlag flags = null;
            try
            {
                if (criteria.IsValid())
                {
                    flags = _userRepository.GetPermissionFlags(ref criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return flags;
        }

        /// <summary>
        /// Res the user check in.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public void ReUserCheckIn(ref UserReCheckInUserCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    _userRepository.UserReCheckIn(ref criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Users the log out.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public void UserLogOut(LogOutUserCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    _userRepository.UserLogOut(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Gets the user information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Object IEnumerable{IUser}.</returns>
        public IEnumerable<IUser> GetUserInfo(GetUserInfoCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    var rawData = _userRepository.GetUserInfo(criteria);
                    var result = new List<User>();
                    foreach (var data in rawData)
                    {
                        result.Add(new User
                        {
                            RoleID = (byte)data.RoleID,
                            FirstName = (string)data.FirstName,
                            LastName = (string)data.LastName,
                            Email = (string)data.Email
                        });
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }
    }
}
