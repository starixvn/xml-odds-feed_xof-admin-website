namespace Fanex.OddsFeed.Core.Service.Interface
{
    using System.Collections.Generic;
    using Fanex.OddsFeed.Core.Criteria.User;
    using Fanex.OddsFeed.Core.Domain;
    using Fanex.OddsFeed.Core.Model;

    public interface IUserService
    {
        IUser UserLogin(ref UserLoginUserCriteria criteria);

        PermissionFlag UserGetProfile(ref UserGetProfileCriteria criteria);

        PermissionFlag GetPermissionFlags(ref UserGetPermissionCriteria criteria);

        void ReUserCheckIn(ref UserReCheckInUserCriteria criteria);

        IEnumerable<IUser> GetUserInfo(GetUserInfoCriteria criteria);
    }
}