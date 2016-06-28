// --------------------------------------------------------------------------
// <copyright file="UserRepository.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.01.01">Created</change>
// </history>
namespace Fanex.OddsFeed.Core.Repository.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.OddsFeed.Core.Business;
    using Fanex.OddsFeed.Core.Criteria.Misc;
    using Fanex.OddsFeed.Core.Criteria.User;
    using Fanex.OddsFeed.Core.Domain;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Model.ValueObject;
    using Fanex.OddsFeed.Core.Repository.Interface;
    using Data.Repository;    //using Fanex.OddsFeed.Framework.Data;

    /// <summary>
    /// Class UserRepository.
    /// </summary>    
    public class UserRepository : RepositoryBase<IUser>, IUserRepository
    {
        /// <summary>
        /// Users the login.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>IUser Object.</returns>
        public IUser UserLogin(ref UserLoginUserCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                Hashtable param = criteria.GetSpParams();
                var objUser = objectDb.QueryEntity<User>(param);

                criteria.SystemIdNameList = param["systemidnamelist"] != null ? param["systemidnamelist"].ToString() : string.Empty;
                criteria.ErrCode = Convert.ToInt32(param["errcode"].ToString());
                criteria.ErrMsg = param["errmsg"] != null ? param["errmsg"].ToString() : string.Empty;

                return objUser;
            }
        }

        /// <summary>
        /// Gets the profile.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Permission Flag.</returns>
        public PermissionFlag GetProfile(ref UserGetProfileCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                Hashtable param = criteria.GetSpParams();
                var objFlag = objectDb.QueryEntity<PermissionFlag>(param);

                criteria.OnUserId = Convert.ToInt32(param["onuserid"].ToString());
                criteria.ErrCode = Convert.ToInt32(param["errcode"].ToString());
                criteria.ErrMsg = param["errmsg"] != null ? param["errmsg"].ToString() : string.Empty;

                return objFlag;
            }
        }

        /// <summary>
        /// Gets the permission flags.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Permission Flag.</returns>
        public PermissionFlag GetPermissionFlags(ref UserGetPermissionCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                return db.QueryEntity<PermissionFlag>(criteria.GetSpParams());
            }
        }

        /// <summary>
        /// Fetches the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="totalRows">The total rows.</param>
        /// <returns>IQuery able {IUser}.</returns>
        public IQueryable<IUser> Fetch(ICriteria criteria, out int totalRows)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                Hashtable hash = criteria.GetSpParams();

                var userList = db.Query<User>(hash);

                totalRows = Convert.ToInt32(hash["totalrows"]);

                return userList.AsQueryable();
            }
        }

        /// <summary>
        /// Users the re check in.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public void UserReCheckIn(ref UserReCheckInUserCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();

            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDb.ExecuteNonQuery(hash);
                criteria.IsKickedOut = Convert.ToBoolean(hash["iskickedout"].ToString());
            }
        }

        /// <summary>
        /// Users the log out.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public void UserLogOut(LogOutUserCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();

            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDb.ExecuteNonQuery(hash);
            }
        }

        /// <summary>
        /// Gets the user information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>IQuery able {dynamic}.</returns>
        public IQueryable<dynamic> GetUserInfo(GetUserInfoCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                Hashtable hash = criteria.GetSpParams();
                var userList = db.Query<dynamic>(hash);
                return userList.AsQueryable();
            }
        }
    }
}