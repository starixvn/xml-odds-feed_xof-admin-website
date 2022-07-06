namespace Fanex.OddsFeed.Core.Repository.Impl
{
    using System;
    using System.Linq;
    using Fanex.Data;
    using Fanex.OddsFeed.Core.Criteria.User;
    using Fanex.OddsFeed.Core.Domain;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Repository.Interface;

    public class UserRepository : IUserRepository
    {
        public IUser UserLogin(ref UserLoginUserCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var param = criteria.GetSpParams();
                var objUser = objectDb.QueryEntity<User>(param);

                criteria.SystemIdNameList = param["systemidnamelist"] != null ? param["systemidnamelist"].ToString() : string.Empty;
                criteria.ErrCode = Convert.ToInt32(param["errcode"].ToString());
                criteria.ErrMsg = param["errmsg"] != null ? param["errmsg"].ToString() : string.Empty;

                return objUser;
            }
        }

        public PermissionFlag GetProfile(ref UserGetProfileCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var param = criteria.GetSpParams();
                var objFlag = objectDb.QueryEntity<PermissionFlag>(param);

                criteria.OnUserId = Convert.ToInt32(param["onuserid"].ToString());
                criteria.ErrCode = Convert.ToInt32(param["errcode"].ToString());
                criteria.ErrMsg = param["errmsg"] != null ? param["errmsg"].ToString() : string.Empty;

                return objFlag;
            }
        }

        public PermissionFlag GetPermissionFlags(ref UserGetPermissionCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                return db.QueryEntity<PermissionFlag>(criteria.GetSpParams());
            }
        }

        public void UserReCheckIn(ref UserReCheckInUserCriteria criteria)
        {
            var hash = criteria.GetSpParams();

            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDb.ExecuteNonQuery(hash);
                criteria.IsKickedOut = Convert.ToBoolean(hash["iskickedout"].ToString());
            }
        }

        public IQueryable<dynamic> GetUserInfo(GetUserInfoCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var hash = criteria.GetSpParams();
                var userList = db.Query<dynamic>(hash);
                return userList.AsQueryable();
            }
        }
    }
}